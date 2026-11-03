# Metro 1 original pixel art v1 — 16px = 1u. All drawn procedurally (no external assets).
import random, json, os, sys
from PIL import Image, ImageDraw
T=16
random.seed(7)
def hx(s): s=s.lstrip('#'); return tuple(int(s[i:i+2],16) for i in (0,2,4))+(255,)
# Palette: 20 colors. Floor A = 아래 거리(따뜻한 나트륨 불빛, 젖은 벽돌). Floor B = 공장(녹, 청록 강철, 증기).
P={
 'void':'#0b0e14','sky0':'#121826','sky1':'#1b2236','sil0':'#1f2638','sil1':'#283149','win':'#e8b45a','winoff':'#3a3550',
 'brk0':'#2a2433','brk1':'#3a3145','brk2':'#4b3f57','mortar':'#1a1520','lit':'#d99a4e','litdim':'#8a5d3a',
 'st0':'#1f2a30','st1':'#2f3f47','st2':'#46606a','rust':'#8a4a2e','rust2':'#b5653a',
 'cat0':'#5b6b78','cat1':'#8aa0ae','acid0':'#2f8f3a','acid1':'#7CFC5A','spike':'#c9ccd6','spk2':'#6c7080',
 'climbY':'#e3b23c','climbK':'#2a2a2a','pipe':'#5d6d74','door':'#c06a2c','doorL':'#ffb36b',
 'dash':'#40e0d0','wj':'#e3b23c','frag':'#7CFC9A','fragL':'#d8ffe6','lantern':'#ffcf6a','steam':'#cfe3ee',
 'player':'#f2c14e','playerD':'#8c5a2b','cloak':'#3d5a80','eye':'#ffffff','enemy':'#c040c0','enemyD':'#5e1f5e'}
C={k:hx(v) for k,v in P.items()}
def tile(): return Image.new('RGBA',(T,T),(0,0,0,0))
def px(im,x,y,c):
    if 0<=x<im.width and 0<=y<im.height: im.putpixel((x,y),c)
def noise(im,cols,amt=0.12):
    for y in range(im.height):
        for x in range(im.width):
            if random.random()<amt: im.putpixel((x,y),random.choice(cols))
tiles={}
# --- A: brick wall body
im=tile(); d=ImageDraw.Draw(im); d.rectangle([0,0,15,15],fill=C['brk1'])
for row in range(4):
    y=row*4; off=0 if row%2==0 else 4
    d.line([0,y+3,15,y+3],fill=C['mortar'])
    for x in range(off,16,8): d.line([x,y,x,y+2],fill=C['mortar'])
noise(im,[C['brk0'],C['brk2']],0.10); tiles['A_wall']=im
# --- A: brick top edge (lit by street lamps)
im=tiles['A_wall'].copy(); d=ImageDraw.Draw(im)
d.rectangle([0,0,15,2],fill=C['brk2']); d.line([0,0,15,0],fill=C['lit']); d.line([0,1,15,1],fill=C['litdim'])
for x in range(0,16,5): px(im,x,2,C['lit'])
tiles['A_top']=im
# --- B: steel plate body
im=tile(); d=ImageDraw.Draw(im); d.rectangle([0,0,15,15],fill=C['st1'])
d.rectangle([0,0,15,15],outline=C['st0']); d.line([1,1,14,1],fill=C['st2'])
for (x,y) in [(2,2),(13,2),(2,13),(13,13)]: px(im,x,y,C['st2'])
for _ in range(6):
    x=random.randint(1,14); y=random.randint(3,14); px(im,x,y,C['rust']); px(im,x,y-1,C['rust2'])
tiles['B_wall']=im
im=tiles['B_wall'].copy(); d=ImageDraw.Draw(im)
d.rectangle([0,0,15,2],fill=C['st2']); d.line([0,0,15,0],fill=C['cat1'])
for x in range(1,16,4): d.line([x,1,x+1,1],fill=C['climbY'])
tiles['B_top']=im
# --- platforms (0.5u thick = 8px): A wooden balcony / B catwalk grating
im=Image.new('RGBA',(T,8),(0,0,0,0)); d=ImageDraw.Draw(im)
d.rectangle([0,0,15,7],fill=C['litdim']); d.line([0,0,15,0],fill=C['lit']); d.line([0,7,15,7],fill=C['brk0'])
for x in (0,8): d.line([x,1,x,6],fill=C['brk0'])
d.line([0,4,15,4],fill=C['rust']); tiles['A_plat']=im
im=Image.new('RGBA',(T,8),(0,0,0,0)); d=ImageDraw.Draw(im)
d.rectangle([0,0,15,7],fill=C['cat0']); d.line([0,0,15,0],fill=C['cat1'])
for x in range(1,16,3): d.line([x,2,x,6],fill=C['st0'])
d.line([0,7,15,7],fill=C['st0']); tiles['B_plat']=im
# crumble: cracked
for k in ('A','B'):
    im=tiles[k+'_plat'].copy(); d=ImageDraw.Draw(im)
    d.line([3,0,6,7],fill=C['void']); d.line([10,0,12,4],fill=C['void']); d.line([12,4,9,7],fill=C['void'])
    tiles[k+'_crumble']=im
# climb wall: dark pipe column with yellow/black hazard stripes on the face
im=tile(); d=ImageDraw.Draw(im); d.rectangle([0,0,15,15],fill=C['pipe'])
d.line([0,0,0,15],fill=C['st0']); d.line([15,0,15,15],fill=C['st0']); d.line([3,0,3,15],fill=C['st2'])
for y in range(0,16,8):
    for i in range(8):
        for x in range(6,12):
            if ((x+y+i)//3)%2==0: px(im,x,y+i,C['climbY'])
            else: px(im,x,y+i,C['climbK'])
tiles['climb']=im
# acid (hazard 0.3u ≈ 5px drawn as 8px band)
im=Image.new('RGBA',(T,8),(0,0,0,0)); d=ImageDraw.Draw(im)
d.rectangle([0,2,15,7],fill=C['acid0'])
for x in range(16): px(im,x,1+(x%4==0),C['acid1'])
for (x,y) in [(3,4),(10,5),(13,3)]: px(im,x,y,C['acid1'])
tiles['acid']=im
# spikes
im=Image.new('RGBA',(T,8),(0,0,0,0)); d=ImageDraw.Draw(im)
for x0 in (0,8): d.polygon([(x0,7),(x0+4,0),(x0+7,7)],fill=C['spike'],outline=C['spk2'])
tiles['spike']=im
# door frame (1x3u): orange frame
im=Image.new('RGBA',(T,48),(0,0,0,0)); d=ImageDraw.Draw(im)
d.rectangle([0,0,15,47],outline=C['door']); d.rectangle([1,1,14,46],outline=C['doorL'])
for y in range(4,46,6): d.line([4,y,11,y],fill=(255,179,107,70))
tiles['door']=im
# fragment (glowing shard)
im=Image.new('RGBA',(T,T),(0,0,0,0)); d=ImageDraw.Draw(im)
d.ellipse([1,1,14,14],fill=(124,252,154,40)); d.polygon([(8,1),(12,8),(8,15),(4,8)],fill=C['frag'],outline=C['fragL'])
d.line([8,3,8,12],fill=C['fragL']); tiles['fragment']=im
# lantern
im=Image.new('RGBA',(T,T),(0,0,0,0)); d=ImageDraw.Draw(im)
d.ellipse([0,0,15,15],fill=(255,207,106,45)); d.rectangle([5,3,10,12],fill=C['lantern'],outline=C['litdim']); d.line([7,0,8,3],fill=C['spk2'])
tiles['lantern']=im
# player (0.7x1.6u -> 11x26): cloaked climber, yellow scarf
im=Image.new('RGBA',(12,26),(0,0,0,0)); d=ImageDraw.Draw(im)
d.ellipse([2,0,9,7],fill=C['player']); d.rectangle([4,3,8,5],fill=C['void']); px(im,7,4,C['eye'])
d.polygon([(1,8),(10,8),(11,22),(0,22)],fill=C['cloak']); d.line([1,8,10,8],fill=C['player']); d.line([2,9,9,9],fill=C['wj'])
d.rectangle([2,22,4,25],fill=C['playerD']); d.rectangle([7,22,9,25],fill=C['playerD'])
tiles['player']=im
# enemy generic 1x1u
im=tile(); d=ImageDraw.Draw(im); d.rounded_rectangle([1,3,14,15],3,fill=C['enemy'],outline=C['enemyD']); d.rectangle([4,7,6,9],fill=C['eye']); d.rectangle([9,7,11,9],fill=C['eye'])
d.line([3,3,1,0],fill=C['enemyD']); d.line([12,3,14,0],fill=C['enemyD']); tiles['enemy']=im
# backgrounds (parallax) 320x180 each
def bg(kind):
    W,H=320,360; im=Image.new('RGBA',(W,H)); d=ImageDraw.Draw(im)
    for y in range(H):
        t=y/H; c=tuple(int(C['sky1'][i]*(1-t)+C['sky0'][i]*t) for i in range(3))+(255,); d.line([0,y,W,y],fill=c)
    rnd=random.Random(3 if kind=='A' else 5)
    x=0
    while x<W:
        w=rnd.randint(18,40); h=rnd.randint(90,300); col=C['sil0'] if rnd.random()<.5 else C['sil1']
        d.rectangle([x,H-h,x+w,H],fill=col)
        if kind=='A':
            for wy in range(H-h+6,H-4,9):
                for wx in range(x+3,x+w-3,6):
                    d.rectangle([wx,wy,wx+2,wy+3],fill=C['win'] if rnd.random()<.25 else C['winoff'])
        else:
            if rnd.random()<.5:
                cx=x+w//2; d.rectangle([cx-3,H-h-30,cx+3,H-h],fill=col)
                ov=Image.new('RGBA',(W,H),(0,0,0,0)); od=ImageDraw.Draw(ov)
                for k in range(3): od.ellipse([cx-6-k*3,H-h-40-k*10,cx+6+k*3,H-h-30-k*10],fill=(120,135,150,40))
                im.alpha_composite(ov)
            for py in range(H-h+10,H,22): d.line([x,py,x+w,py],fill=C['rust'])
        x+=w+rnd.randint(0,6)
    return im

# ---- v1.1 sprites: distinct enemies, bosses, pickups, checkpoint, lever
def spr(w,h): im=Image.new('RGBA',(w,h),(0,0,0,0)); return im,ImageDraw.Draw(im)
E=C['enemy'];ED=C['enemyD'];EYE=C['eye']
im,d=spr(16,16); d.ellipse([1,6,14,15],fill=E,outline=ED); [d.line([x,15,x-1,13],fill=ED) for x in (4,8,12)]; d.rectangle([5,9,6,10],fill=EYE); d.rectangle([9,9,10,10],fill=EYE); tiles['E1']=im  # 순찰 딱정벌레
im,d=spr(16,16); d.polygon([(1,15),(3,5),(13,5),(15,15)],fill=E,outline=ED); d.polygon([(12,6),(16,2),(14,7)],fill=C['spike']); d.rectangle([4,8,6,10],fill=C['acid1']); tiles['E2']=im  # 돌진(뿔)
im,d=spr(16,16); d.rectangle([3,8,12,15],fill=ED); d.ellipse([3,3,12,12],fill=E,outline=ED); d.rectangle([11,6,15,8],fill=C['spk2']); px(im,7,7,C['acid1']); tiles['E3']=im  # 포탑
im,d=spr(16,16); d.ellipse([4,5,11,13],fill=E,outline=ED); d.ellipse([0,2,6,7],fill=(207,227,238,150)); d.ellipse([9,2,15,7],fill=(207,227,238,150)); px(im,6,8,EYE); px(im,9,8,EYE); tiles['E4']=im  # 벌레(포고)
im,d=spr(48,48); d.rounded_rectangle([4,12,44,47],6,fill=E,outline=ED); d.polygon([(8,12),(4,0),(16,10)],fill=C['spike']); d.polygon([(40,12),(44,0),(32,10)],fill=C['spike'])
d.rectangle([12,22,18,27],fill=C['win']); d.rectangle([30,22,36,27],fill=C['win']); d.line([14,36,34,36],fill=ED,width=2); tiles['boss1']=im  # 폐수로 짐승
im,d=spr(48,64); d.rectangle([10,10,38,63],fill=C['st1'],outline=C['st0']); d.rectangle([4,20,10,58],fill=C['pipe']); d.rectangle([38,20,44,58],fill=C['pipe'])
for y in range(22,58,6): d.line([4,y,10,y],fill=C['climbY']); d.line([38,y,44,y],fill=C['climbY'])
d.ellipse([16,14,32,30],fill=C['rust2'],outline=C['rust']); d.ellipse([21,19,27,25],fill=C['win']); d.rectangle([14,40,34,44],fill=C['rust']); tiles['boss2']=im  # 벽 감시자(보일러)
im,d=spr(16,16); d.ellipse([1,1,14,14],fill=(64,224,208,60)); d.ellipse([4,4,11,11],fill=C['dash']); d.polygon([(5,8),(11,8),(9,6)],fill=C['eye']); tiles['pickup_dash']=im
im,d=spr(16,16); d.ellipse([1,1,14,14],fill=(227,178,60,60)); d.rectangle([5,3,10,13],fill=C['climbY']); [d.line([5,y,10,y],fill=C['climbK']) for y in (5,8,11)]; tiles['pickup_walljump']=im
im,d=spr(16,32); d.rectangle([7,4,8,31],fill=C['spk2']); d.ellipse([3,0,12,8],fill=C['win']); d.ellipse([0,-2,15,11],fill=(232,180,90,50)); d.rectangle([1,26,14,28],fill=C['litdim']); tiles['checkpoint']=im  # 가로등 쉼터
im,d=spr(16,16); d.rectangle([3,11,12,15],fill=C['st0']); d.line([7,11,11,3],fill=C['spk2'],width=2); d.ellipse([9,1,13,5],fill=C['door']); tiles['lever']=im

os.chdir(os.path.dirname(os.path.abspath(__file__))); os.makedirs('tiles',exist_ok=True)
for k,v in tiles.items(): v.save(f'tiles/{k}.png')
from PIL import ImageEnhance
for k in 'AB':
    b=bg(k); b=ImageEnhance.Brightness(b).enhance(0.55); b=ImageEnhance.Color(b).enhance(0.6); b.save(f'bg_{k}.png')
json.dump(P,open('palette.json','w'),indent=1)
print(len(tiles),'tiles')
