using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
[InitializeOnLoad]
public static class C4Verification
{
    static C4Verification(){EditorApplication.playModeStateChanged+=s=>{if(!SessionState.GetBool("C4.Run",false))return;if(s==PlayModeStateChange.EnteredPlayMode)new GameObject("C4 persistent verification").AddComponent<C4Runner>();if(s==PlayModeStateChange.EnteredEditMode){SessionState.SetBool("C4.Run",false);EditorApplication.Exit(SessionState.GetInt("C4.Exit",1));}};}
    public static void Run(){string path=Nightly.Tag.Contains("rerun")?"Validation/Verification-T4-retry-rerun.txt":"Validation/Verification-T4-retry.txt";if(File.Exists(path))throw new IOException("Preserve evidence");SessionState.SetString("C4.Path",path);SessionState.SetBool("C4.Run",true);EditorSceneManager.OpenScene("Assets/Scenes/Test/CombatTest.unity");EditorApplication.delayCall+=()=>EditorApplication.isPlaying=true;}
}
public sealed class C4Runner:MonoBehaviour
{
    readonly List<string> lines=new();readonly List<string> errors=new();
    void Check(bool ok,string msg){if(!ok)throw new Exception(msg);lines.Add("PASS: "+msg);}
    void Log(string msg,string stack,LogType type){if(type is LogType.Error or LogType.Exception or LogType.Assert)errors.Add(msg+stack);}
    IEnumerator Start(){DontDestroyOnLoad(gameObject);Application.logMessageReceived+=Log;var stack=new Stack<IEnumerator>();stack.Push(Tests());bool fail=false;while(stack.Count>0&&!fail){object next=null;try{if(!stack.Peek().MoveNext()){stack.Pop();continue;}next=stack.Peek().Current;}catch(Exception e){fail=true;lines.Add(e.ToString());}if(next is IEnumerator child)stack.Push(child);else if(!fail)yield return next;}Application.logMessageReceived-=Log;fail|=errors.Count>0;lines.AddRange(errors);lines.Insert(0,(fail?"FAIL":"PASS")+" C4 E1 Unity="+Application.unityVersion);lines.Add("FAIL "+(fail?1:0));File.WriteAllLines(SessionState.GetString("C4.Path",""),lines);SessionState.SetInt("C4.Exit",fail?1:0);EditorApplication.isPlaying=false;}
    void PlacePlayer(Vector2 at){var player=RoomSession.Instance.Player;var body=player.GetComponent<Rigidbody2D>();player.ClearTransientInput();player.transform.position=at;body.position=at;body.linearVelocity=Vector2.zero;Physics2D.SyncTransforms();}
    void PlaceEnemy(EnemyPatrol enemy,Vector2 at){enemy.transform.position=at;var body=enemy.GetComponent<Rigidbody2D>();body.position=at;body.linearVelocity=Vector2.zero;Physics2D.SyncTransforms();}
    IEnumerator Key(Keyboard kb,params Key[] keys){InputSystem.QueueStateEvent(kb,new KeyboardState(keys));yield return null;}
    IEnumerator Attack(Keyboard kb){yield return Key(kb,UnityEngine.InputSystem.Key.D);yield return new WaitForFixedUpdate();yield return Key(kb,UnityEngine.InputSystem.Key.J);yield return Key(kb);yield return new WaitForSeconds(.21f);}
    IEnumerator Tests()
    {
        InputSystem.settings.editorInputBehaviorInPlayMode=InputSettings.EditorInputBehaviorInPlayMode.AllDeviceInputAlwaysGoesToGameView;
        InputSystem.settings.backgroundBehavior=InputSettings.BackgroundBehavior.IgnoreFocus;
        var kb=InputSystem.AddDevice<Keyboard>();yield return new WaitForSeconds(.3f);
        var session=RoomSession.Instance;var player=session.Player;var health=player.GetComponent<PlayerHealth>();var spawner=FindFirstObjectByType<EnemyRespawn>();var enemy=spawner.live;var body=enemy.GetComponent<Rigidbody2D>();
        Check(FindObjectsByType<EnemyPatrol>(FindObjectsSortMode.None).Length==1&&enemy.HP==2&&enemy.tuning.speed==1.8f&&enemy.tuning.knockback==3&&enemy.tuning.contactDamage==1,"one CombatTest E1; exact 22-section values");
        // Reproduce the old test relocation method before changing it: body-only move.
        enemy.enabled=false;PlaceEnemy(enemy,new Vector2(34,1.5f));body.position=new Vector2(20,1.5f);
        lines.Add($"MEASURE old body-only relocation body={body.position} Transform={enemy.transform.position} collider={enemy.GetComponent<BoxCollider2D>().bounds}");
        PlacePlayer(new Vector2(enemy.transform.position.x-1.25f,1.81f));
        var bounds=player.GetComponent<BoxCollider2D>().bounds;var attack=player.GetComponent<PlayerAttack>();
        lines.Add($"MEASURE old attack box x={bounds.max.x:F3}..{bounds.max.x+attack.tuning.attackRange:F3}, y={bounds.center.y-attack.tuning.attackHeight/2:F3}..{bounds.center.y+attack.tuning.attackHeight/2:F3}; enemy collider={enemy.GetComponent<BoxCollider2D>().bounds}; HP before={enemy.HP}, velocity before={body.linearVelocity}");
        yield return Attack(kb);
        lines.Add($"MEASURE old relocation attack HP after={enemy.HP}, velocity after={body.linearVelocity}, instant hit velocity={enemy.LastHitVelocity}; body={body.position}, Transform={enemy.transform.position}");
        lines.Add(enemy.HP==2?"CAUSE reproduction: body-only relocation used a stale Transform to place attacker outside the actual hit range. Correct test setup synchronizes both positions.":"CAUSE old first failure not reproduced; original saved evidence cannot establish the exact HP/velocity cause. No game value change.");
        // Correct the measured test placement; no combat tuning changes.
        health.RestoreMax();yield return new WaitForSeconds(.4f);enemy=spawner.live;body=enemy.GetComponent<Rigidbody2D>();enemy.enabled=false;
        PlaceEnemy(enemy,new Vector2(20,1.5f));PlacePlayer(new Vector2(18.75f,1.81f));
        bounds=player.GetComponent<BoxCollider2D>().bounds;
        lines.Add($"MEASURE synchronized attack box x={bounds.max.x:F3}..{bounds.max.x+attack.tuning.attackRange:F3}; E1 collider={enemy.GetComponent<BoxCollider2D>().bounds}; HP before={enemy.HP}; velocity before={body.linearVelocity}");
        yield return Attack(kb);
        lines.Add($"MEASURE first hit HP {enemy.LastHitHPBefore}->{enemy.HP}, instant velocity={enemy.LastHitVelocity}, sampled velocity={body.linearVelocity}");
        Check(enemy.HP==1&&Mathf.Abs(enemy.LastHitVelocity.x-3)<.001f,"actual first attack loses one HP and applies 3u/s knockback");
        yield return new WaitForSeconds(.4f);PlacePlayer(new Vector2(body.position.x-1.25f,1.81f));yield return Attack(kb);
        Check(enemy==null,"second actual attack removes E1 (no duplicate damage on one attack)");
        health.RestoreMax();yield return new WaitForSeconds(.1f);enemy=spawner.live;body=enemy.GetComponent<Rigidbody2D>();PlacePlayer(new Vector2(6,1.81f));
        int turns=enemy.Turns;float low=body.position.x,high=low;float end=Time.time+9;while(Time.time<end){low=Mathf.Min(low,body.position.x);high=Mathf.Max(high,body.position.x);yield return null;}
        lines.Add($"MEASURE patrol x={low:F3}..{high:F3} turns={enemy.Turns-turns}");
        Check(enemy.Turns-turns>=2&&low<16.2f&&high>22.8f,"patrol round trip between Scene endpoints");
        spawner.rightPoint.position=new Vector3(40,1.5f,0);PlaceEnemy(enemy,new Vector2(34,1.5f));turns=enemy.Turns;yield return new WaitForSeconds(.8f);
        Check(enemy.Turns>turns&&body.position.x<34.51f,"ordinary wall turn before penetration");spawner.rightPoint.position=new Vector3(23,1.5f,0);
        var ledge=Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/RoomBlock.prefab"));ledge.transform.position=new Vector3(50,5,0);ledge.transform.localScale=new Vector3(6,1,1);spawner.leftPoint.position=new Vector3(42,6,0);spawner.rightPoint.position=new Vector3(58,6,0);PlaceEnemy(enemy,new Vector2(50,6.01f));turns=enemy.Turns;
        low=50;high=50;end=Time.time+5;while(Time.time<end){low=Mathf.Min(low,body.position.x);high=Mathf.Max(high,body.position.x);yield return null;}
        lines.Add($"MEASURE ledge x={low:F3}..{high:F3} y={body.position.y:F3} turns={enemy.Turns-turns}");Check(enemy.Turns>turns&&body.position.y>5.8f&&low>47&&high<53,"ledge turns without falling");Destroy(ledge);spawner.leftPoint.position=new Vector3(16,1.5f,0);spawner.rightPoint.position=new Vector3(23,1.5f,0);
        enemy.enabled=false;PlaceEnemy(enemy,new Vector2(20,1.5f));health.RestoreMax();yield return null;enemy=spawner.live;enemy.enabled=false;PlaceEnemy(enemy,new Vector2(20,1.5f));PlacePlayer(new Vector2(19.15f,1.81f));yield return new WaitForSeconds(.1f);
        Check(health.HP==4&&health.Invincible,"physical E1 contact deals one HP and starts invincibility");PlacePlayer(new Vector2(19.15f,1.81f));yield return new WaitForSeconds(.1f);Check(health.HP==4,"repeated contact during invincibility cannot duplicate damage");
        PlacePlayer(new Vector2(6,1.81f));PlaceEnemy(enemy,new Vector2(26.48f,1.5f));enemy.ReceiveHit(1,new Vector2(25,1.5f));yield return new WaitForSeconds(.15f);Check(enemy==null,"E1 knocked into KillZone is removed");
        Check(session.Die(),"player death requested");yield return new WaitForSecondsRealtime(.85f);enemy=spawner.live;Check(enemy!=null&&enemy.HP==2&&FindObjectsByType<EnemyPatrol>(FindObjectsSortMode.None).Length==1,"player death restores one full-HP E1");
        Check(session.RequestTransition("DashTest","Assets/Scenes/Test/DashTest.unity","Entry"),"test room departure accepted");while(session.Transitioning)yield return null;
        Check(this!=null&&FindObjectsByType<EnemyPatrol>(FindObjectsSortMode.None).Length==0,"verification survives departure; previous E1 cleaned up");
        Check(session.RequestTransition("CombatTest","Assets/Scenes/Test/CombatTest.unity","Entry"),"test room reentry accepted");while(session.Transitioning)yield return null;yield return new WaitForSeconds(.2f);
        enemy=FindFirstObjectByType<EnemyPatrol>();Check(enemy!=null&&enemy.HP==2&&FindObjectsByType<EnemyPatrol>(FindObjectsSortMode.None).Length==1,"room reentry resets one full-HP E1");InputSystem.RemoveDevice(kb);
    }
}
