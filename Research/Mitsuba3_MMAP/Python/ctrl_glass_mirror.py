import time
import myUtility as util
import xml.etree.ElementTree as ET
#import xmlManipulator as xmlManip

## Properties
# filaNames
xml = "../scenes/main.xml"
#xml = "../scenes/display.xml"
outputFolderName = "flower_gl0-0.003_mi0-0.002"
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

origins = []
for i in range(1):
    origins.append([0.1*i,0,-80])
    origins.append([-0.1*i,0,-80])
origins = [[0,0,-80]]

glass_roughs = []
for i in range(30):
    glass_roughs.append(0.0001*i)

mirror_roughs = []
for i in range(20):
    mirror_roughs.append(0.0001*i)

sensors = [util.load_sensor(origin) for origin in origins]

startTime = time.time()                 # tic
#util.runRender(xml, outputFileName, usingVariant, True, 8, True, True)
for i, sensor in enumerate(sensors):
    for j, gl_rough in enumerate(glass_roughs):
        for bsdf in root.iter('bsdf'):
            if bsdf.get('id') == ctrl_id1:
                for param in bsdf:
                    if param.tag == 'float' and param.get('name') == ('alpha'):
                        param.set('value', str(gl_rough))
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
    
            util.Rough_Run(xmlfile='../scenes/_render.xml', savepath=outputFileName, mysensor=sensor, deg=origins[i][0], gl_rou=glass_roughs[j], mi_rou=mirror_roughs[k], variant=usingVariant, gamma=True, UIntVal=8, saveEXR=True, overWrite=True)


#print("SUCCESS")
renderingTime = time.time()-startTime   # toc
renderingTime = 0 if renderingTime<0.1 else renderingTime   # thresholding

elapses.append(renderingTime)  # toc
util.notify("Render done", "Check whether MMAP is working")
#util.notify_render(elapses, notes="Check whether MMAP is working")
