import time
import myUtility as util
#import xmlManipulator as xmlManip

## Properties
# filaNames
xml = "../scenes/nocutmesh.xml"
#xml = "../scenes/display.xml"
outputFolderName = "nocut"
outputFileName = "result"
#usingVariant = "scalar_rgb"
usingVariant = "llvm_ad_rgb"
#usingVariant = "cuda_ad_rgb"

#util.dr.jit_init(JitBackendCUDA)

# mkdir
dn = util.mkdirToday()
outputdir = util.mkdir(dn+"/"+outputFolderName)
outputFileName = outputdir+"/"+outputFileName+".png"

util.set_variant(usingVariant)

# render
elapses = []    # rec

origins = []
for i in range(1):
    origins.append([0.1*i,0,-80])
    origins.append([-0.1*i,0,-80])

sensors = [util.load_sensor(origin) for origin in origins]

startTime = time.time()                 # tic
#util.runRender(xml, outputFileName, usingVariant, True, 8, True, True)
for i, sensor in enumerate(sensors):
    util.Run(xmlfile=xml, savepath=outputFileName, mysensor=sensor, deg=origins[i][0], variant=usingVariant, gamma=True, UIntVal=8, saveEXR=True, overWrite=True)


#print("SUCCESS")
renderingTime = time.time()-startTime   # toc
renderingTime = 0 if renderingTime<0.1 else renderingTime   # thresholding

elapses.append(renderingTime)  # toc
util.notify("Render done", "Check whether MMAP is working")
#util.notify_render(elapses, notes="Check whether MMAP is working")
