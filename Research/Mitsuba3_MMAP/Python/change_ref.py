import time
import myUtility as util
#import xmlManipulator as xmlManip

## Properties
# filaNames
xml = "../scenes/MMAP2.xml"
#xml = "../scenes/display.xml"
outputFolderName = "change_ref0.87"
outputFileName = "result0.87"
#usingVariant = "scalar_rgb"
usingVariant = "llvm_ad_rgb"
#usingVariant = "cuda_ad_rgb"

#util.dr.jit_init(JitBackendCUDA)

# mkdir
dn = util.mkdirToday()
outputdir = util.mkdir(dn+"/"+outputFolderName)
outputFileName = outputdir+"/"+outputFileName+".png"

# render
elapses = []    # rec

startTime = time.time()                 # tic
#util.runRender(xml, outputFileName, usingVariant, True, 8, True, True)
util.Run(xml, outputFileName, usingVariant, True, 8, True, True)


#print("SUCCESS")
renderingTime = time.time()-startTime   # toc
renderingTime = 0 if renderingTime<0.1 else renderingTime   # thresholding

elapses.append(renderingTime)  # toc
util.notify("Render done", "Check whether MMAP is working")
#util.notify_render(elapses, notes="Check whether MMAP is working")
