import time
import myUtility as util
import xml.etree.ElementTree as ET
#import xmlManipulator as xmlManip

## Properties
# filaNames
xml = "../scenes/multiSceneForMTF.xml"
#xml = "../scenes/display.xml"
outputFolderName = "flower_dis15.30.45_gl0.0006-0_mi0-0.0004"
outputFileName = "result"
#usingVariant = "scalar_rgb"
usingVariant = "llvm_ad_rgb"
#usingVariant = "cuda_ad_rgb"

#util.dr.jit_init(JitBackendCUDA)

ctrl_id1 = 'MMAP_Glass'
ctrl_id2 = 'Mirror'


# mkdir
dn = util.mkdirToday()
outputdir = util.mkdir(dn+"/"+outputFolderName)
outputFileName = outputdir+"/"+outputFileName+".png"

tree = ET.parse(xml)
root = tree.getroot()


util.set_variant(usingVariant)

# render
elapses = []    # rec

#origins = []
#for i in range(1):
#    origins.append([0.1*i,0,-80])
#    origins.append([-0.1*i,0,-80])
#origins = [[0,0,-60]]

glass_roughs = []
for i in range(4):
    glass_roughs.append(0.001*i)

mirror_roughs = []
for i in range(3):
    mirror_roughs.append(0.001*i)


glass_roughs = [0.0006] #[0.003, 0.004]
mirror_roughs = [0.0004] #[0.002, 0.003]

#print(mirror_roughs)
#quit()
sens = [30]
display_distance = [15, 30, 45]#[15, 20, 25, 30, 35, 40, 45]
sample_count = 1024
render_w = 2160
render_h = 1440


#sensors = [util.load_sensor(origin) for origin in origins]

startTime = time.time()                 # tic
#util.runRender(xml, outputFileName, usingVariant, True, 8, True, True)
for i, dd in enumerate(display_distance):
    now_dd = dd
    for sen in root.iter('sensor'):
        for param in sen:
            #print(param)
            #if param.tag == 'float' and param.get('name') == ('focus_distance'):
            #    param.set('value', str(dd*10))
            if param.tag == 'transform' and param.get('name') == ('to_world'):
                for trans in param.iter('transform'):
                    lookat = trans.find('lookat')
                    #print(lookat)
                    if lookat is not None:
                        lookat.set('origin', f"0, 0, {-1*(dd+30)}")
                        lookat.set('target', f"0, 0, {-1*dd}")
            for samp in param.iter('sampler'):
                integer = samp.find('integer')
                #print(integer)
                    #quit()
                if integer is not None:
                    integer.set('value', str(sample_count))
                #if param.tag == 'integer' and param.get('name') == ('sample_count'):
                #        param.set('value', str(dd))
            for film in param.iter('film'):
                for inte in film.iter('integer'):
                    if inte.tag == 'integer' and inte.get('name') == ('width'):
                        inte.set('value', str(render_w))
                    if inte.tag == 'integer' and inte.get('name') == ('height'):
                        inte.set('value', str(render_h))
    for sha in root.iter('shape'):
        #print(sha)
        if sha.get('id') == 'Dis':
            for param in sha:
                #print(param)
                if param.tag == 'transform' and param.get('name') == ('to_world'):
                    for trans in param.iter('transform'):
                        translate = param.find('translate')
                        if translate is not None:
                            translate.set('y', str(-1 * dd))
                        
                        #print(translate)
                
                #if integer is not None:
                    #integer.set('value', str(render_w))

    for j, gl_rough in enumerate(glass_roughs):
        for bsdf in root.iter('bsdf'):
            if bsdf.get('id') == ctrl_id1:
                for param in bsdf:
                    #print(param)
                    if param.tag == 'float' and param.get('name') == ('alpha'):
                        param.set('value', str(gl_rough))
        #quit()
        for k, mi_rough in enumerate(mirror_roughs):

        #for bsdf in root.iter('bsdf'):
        #    if bsdf.get('id') == ctrl_id:
        #        for param in bsdf:
        #            if param.tag == 'float' and param.get('name') == ('alpha'):
        #                param.set('value', str(rough))
            for bsdf in root.iter('bsdf'):
                if bsdf.get('id') == ctrl_id2:
                    for param in bsdf:
                        if param.tag == 'float' and param.get('name') == ('alpha'):
                            param.set('value', str(mi_rough))
                #print(bsdf.get('id'))
            #quit()
            tree.write('../scenes/_render.xml')
            #print(display_distance[i])
            #print(dd)
    
            util.Multi_Rough_Run(xmlfile='../scenes/_render.xml', savepath=outputFileName, cam_display_dis=now_dd, deg=0, gl_rou=glass_roughs[j], mi_rou=mirror_roughs[k], variant=usingVariant, gamma=True, UIntVal=8, saveEXR=True, overWrite=True)


#print("SUCCESS")
renderingTime = time.time()-startTime   # toc
renderingTime = 0 if renderingTime<0.1 else renderingTime   # thresholding

elapses.append(renderingTime)  # toc
util.notify("Render done", "Check whether MMAP is working")
#util.notify_render(elapses, notes="Check whether MMAP is working")
