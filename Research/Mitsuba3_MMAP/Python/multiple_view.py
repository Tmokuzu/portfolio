import time
import myUtility as util
#from mitsuba import ScalarTransform4f as T
import mitsuba as mi
#import xmlManipulator as xmlManip

## Properties
# filaNames
xml = "../scenes/multiple_view.xml"
#xml = "../scenes/display.xml"
outputFolderName = "multiple_view-0.2-0.2"
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

sensor_count = 4
radius = 10
phis = [20.0 * i for i in range(sensor_count)]
theta = 60.0

origins = []
for i in range(2):
    origins.append([0.1*i,0,-80])
    origins.append([-0.1*i,0,-80])
origins = [[0.1,0,-80],[0.3,0,-80]]
#print(origins)
#quit()
#origin = T.rotate([0, 0, 1], phi).rotate([0, 1, 0], theta) @ mi.ScalarPoint3f([0, 0, r])

min_deg, max_deg = -30, 30
deg = min_deg
deg_per_frame = 1


sensors = [util.load_sensor(origin) for origin in origins]
#print(len(sensors))
#quit()

startTime = time.time()                 # tic
#util.runRender(xml, outputFileName, usingVariant, True, 8, True, True)
for i, sensor in enumerate(sensors):
    util.Run(xml, outputFileName, sensor, origins[i][0], usingVariant, True, 8, True, True)

#print("SUCCESS")
renderingTime = time.time()-startTime   # toc
renderingTime = 0 if renderingTime<0.1 else renderingTime   # thresholding

elapses.append(renderingTime)  # toc
util.notify("Render done", "Check whether MMAP is working")
#util.notify_render(elapses, notes="Check whether MMAP is working")