import json,sys
import os; os.chdir(os.path.dirname(os.path.abspath(__file__)))
from PIL import Image
T=16; import sys; G=sys.argv[1] if len(sys.argv)>1 else '../v5/rooms_v5_AB.json'
R=json.load(open(G))['rooms']; t={k:Image.open(f'tiles/{k}.png') for k in ['A_wall','A_top','B_wall','B_top','A_plat','B_plat','A_crumble','B_crumble','climb','acid','spike','door','fragment','lantern','player','enemy']}
def render(rid,player=None):
    r=R[rid]; w,h=r['size']; fl=rid[0]; W,H=int(w*T),int(h*T)
    bg=Image.open(f'bg_{fl}.png').resize((W,H),Image.NEAREST); im=bg.copy()
    Y=lambda y:H-int(round(y*T))
    def fill(rect,tl,topl=None):
        x,y,ww,hh=rect; x0,x1=int(round(x*T)),int(round((x+ww)*T)); ytop,ybot=Y(y+hh),Y(y)
        for py in range(ytop,ybot,T):
            for pxx in range(x0,x1,T):
                src=topl if (topl is not None and py==ytop) else tl
                cw,ch=min(T,x1-pxx),min(T,ybot-py)
                im.alpha_composite(src.crop((0,0,cw,ch)),(pxx,py))
    climbs=[tuple(c) for c in r.get('climb',[])]
    for s in r['solid']: fill(s,t[fl+'_wall'],t[fl+'_top'])
    for c in climbs: fill(c,t['climb'])
    cr=set(r.get('crumble',[]))
    for i,p in enumerate(r['platform']):
        x,y,ww,hh=p; x0,x1=int(round(x*T)),int(round((x+ww)*T)); src=t[fl+('_crumble' if i in cr else '_plat')]
        for pxx in range(x0,x1,T): im.alpha_composite(src.crop((0,0,min(T,x1-pxx),8)),(pxx,Y(y+hh)))
    for hz in r.get('hazard',[]):
        x,y,ww,hh=hz
        for pxx in range(int(x*T),int((x+ww)*T),T): im.alpha_composite(t['acid'].crop((0,0,min(T,int((x+ww)*T)-pxx),8)),(pxx,Y(y+hh)-3))
    for L in r.get('lifts',[]):
        x0,x1=int(L['x0']*T),int(L['x1']*T)
        for pxx in range(x0,x1,T): im.alpha_composite(t['B_plat'].crop((0,0,min(T,x1-pxx),8)),(pxx,Y(L['lo'])))
    for v in r.get('steam',[]):
        ov=Image.new('RGBA',(int((v['x1']-v['x0'])*T),int((v['top']-v['y0'])*T)),(207,227,238,55)); im.alpha_composite(ov,(int(v['x0']*T),Y(v['top'])))
    for d in r['doors']:
        x,y,ww,hh=d['rect']
        if hh>=ww: im.alpha_composite(t['door'].crop((0,0,16,int(hh*T))),(int(x*T),Y(y+hh)))
    for p in r.get('pogo',[]):
        src=t['lantern'] if p['kind']=='lantern' else t['spike'].resize((16,8))
        im.alpha_composite(src,(int(p['x']*T)-8,Y(p['y'])-12))
    for e in r['enemies']: im.alpha_composite(t['enemy'],(int(e['x']*T)-8,Y(e['y'])-16))
    for m in r.get('marks',[]):
        if '조각' in m['t']: im.alpha_composite(t['fragment'],(int(m['x']*T)-8,Y(m['y'])-8))
    if player: im.alpha_composite(t['player'],(int(player[0]*T)-6,Y(player[1])-26))
    return im
out=[]
for rid,pl in [('A03',(10,1)),('B02',(5,1)),('B03',(4,8)),('B01',(22,1))]:
    im=render(rid,pl); im=im.resize((im.width*2,im.height*2),Image.NEAREST); im.save(f'mockups/mock_{rid}.png'); print(rid,im.size)
