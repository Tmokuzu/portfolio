import time
import myUtility as util

## Properties
# fileNames
xml = "../scenes/MMAP.xml"
outputFolderName = "first_MMAP"
outputFileName = "result"
usingVariant = "llvm_ad_rgb"
#usingVariant = "cuda_ad_rgb"

# mkdir
dn = util.mkdirToday()
outputdir = util.mkdir(dn+"/"+outputFolderName)
outputFileName = outputdir+"/"+outputFileName+".png"

# render
elapses = []    # rec

startTime = time.time()                 # tic
util.Run(xml, outputFileName, usingVariant, True, 8, True, True)


renderingTime = time.time()-startTime   # toc
renderingTime = 0 if renderingTime<0.1 else renderingTime   # thresholding

elapses.append(renderingTime)  # toc
util.notify("Render done", "Check whether MMAP is working")