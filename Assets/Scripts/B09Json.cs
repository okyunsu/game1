using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
// Design-only JSON reader. No package or runtime dependency added.
public sealed class B09Json
{
 readonly string text;int at;
 B09Json(string value){text=value;}
 public static Dictionary<string,object> Read(string value)=>(Dictionary<string,object>)new B09Json(value).Value();
 void Space(){while(at<text.Length&&char.IsWhiteSpace(text[at]))at++;}
 object Value(){Space();char c=text[at];if(c=='{')return Map();if(c=='[')return List();if(c=='"')return String();if(c=='t'){at+=4;return true;}if(c=='f'){at+=5;return false;}if(c=='n'){at+=4;return null;}int start=at;while(at<text.Length&&"0123456789.eE+-".IndexOf(text[at])>=0)at++;return double.Parse(text.Substring(start,at-start),CultureInfo.InvariantCulture);}
 Dictionary<string,object> Map(){var result=new Dictionary<string,object>();at++;Space();if(text[at]=='}'){at++;return result;}while(true){Space();string key=String();Space();if(text[at++]!=':')throw new FormatException("JSON colon");result.Add(key,Value());Space();char c=text[at++];if(c=='}')return result;if(c!=',')throw new FormatException("JSON comma");}}
 List<object> List(){var result=new List<object>();at++;Space();if(text[at]==']'){at++;return result;}while(true){result.Add(Value());Space();char c=text[at++];if(c==']')return result;if(c!=',')throw new FormatException("JSON comma");}}
 string String(){if(text[at++]!='"')throw new FormatException("JSON string");var result=new StringBuilder();while(at<text.Length){char c=text[at++];if(c=='"')return result.ToString();if(c=='\\'){c=text[at++];if(c=='u'){result.Append((char)int.Parse(text.Substring(at,4),NumberStyles.HexNumber));at+=4;continue;}c=c=='n'?'\n':c=='r'?'\r':c=='t'?'\t':c=='b'?'\b':c=='f'?'\f':c;}result.Append(c);}throw new FormatException("JSON end");}
 public static Dictionary<string,object> D(object x)=>(Dictionary<string,object>)x;
 public static List<object> A(Dictionary<string,object> d,string key)=>d.TryGetValue(key,out var v)?(List<object>)v:new List<object>();
 public static float F(object x)=>Convert.ToSingle(x,CultureInfo.InvariantCulture);
 public static float F(Dictionary<string,object> d,string key,float fallback=0)=>d.TryGetValue(key,out var v)?F(v):fallback;
 public static string S(Dictionary<string,object>d,string key,string fallback="")=>d.TryGetValue(key,out var v)?Convert.ToString(v,CultureInfo.InvariantCulture):fallback;
}
