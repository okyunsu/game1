# approach head-bump scan: standing surface S, overhead body P with 0 < P.bottom - S.top < 4.12 (jump head),
# horizontally beside S (no overlap) within gap < 1.8  OR overlapping (low ceiling).
import json,sys
d=json.load(open(sys.argv[1]))['rooms']
H=2.52+1.6
for k,r in d.items():
    plats=[('P',i,p) for i,p in enumerate(r['platform'])]
    sol=[('S',i,s) for i,s in enumerate(r['solid'])]
    W,Hh=r['size']
    surfaces=[(t,i,x,y+h,x+w) for t,i,(x,y,w,h) in plats+sol if y+h<Hh-0.5]
    for (t,i,x0,top,x1) in surfaces:
        for (t2,j,(x,y,w,h)) in plats+sol:
            if (t2,j)==(t,i): continue
            clr=y-top
            if not (0<clr<H): continue
            if x<x1 and x+w>x0:
                # overlap: low ceiling only if standing space exists under it on S
                ov=min(x1,x+w)-max(x0,x)
                if ov>0.7 and clr<H-0.3: print(f"{k} CEIL {t}{i} top={top} under {t2}{j} bottom={y} clr={clr:.2f} overlap={ov:.1f}")
            else:
                gap=min(abs(x-x1),abs(x0-(x+w)))
                if gap<1.8 and clr<2.6+0.5 and (y+h)-top<=2.6: print(f"{k} APPROACH {t}{i}[{x0},{x1}]top={top} -> {t2}{j}[{x},{x+w}] bottom={y} top={y+h} gap={gap:.2f} rise={y+h-top:.2f}")
