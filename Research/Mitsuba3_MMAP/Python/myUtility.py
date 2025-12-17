import cv2 as cv
import os 
import numpy as np 
import matplotlib.pyplot as plt
import drjit as dr
import mitsuba as mi
import datetime
import math as mathpy
import random
import xmlManipulator as xmlManip
import myUtility as util
import time
import ntfy.backends.pushbullet as pb
import ntfy.backends.pushover as po


# acces token of my smartphone
pb_access_token = "ui1myyf4tpf9otvwhnh6tybzp2n1e9"

# mitsuba.set_variant('scalar_rgb')
# from mitsuba.core import Thread, math, Properties, Frame3f, Float, Vector3f, warp, Bitmap, Struct
# from mitsuba.core.xml import load_file, load_string
# from mitsuba.render import BSDF, BSDFContext, BSDFFlags, BSDFSample3f, SurfaceInteraction3f, \
#                         register_bsdf, Texture, MicrofacetDistribution, reflect, MicrofacetType

# mitsuba.set_variant('gpu_rgb')
# from mitsuba.core import Thread, math, Properties, Frame3f, Float, Vector3f, warp, Bitmap, Struct
# from mitsuba.core.xml import load_file, load_string
# from mitsuba.render import BSDF, BSDFContext, BSDFFlags, BSDFSample3f, SurfaceInteraction3f, \
#                         register_bsdf, Texture, MicrofacetDistribution, reflect, MicrofacetType

##############################
##          Notify          ##
##############################

# send notify to my smartphone
def notify(title, message):
    po.notify(title, message, pb_access_token)
    return

# send average rendering time, and number of rendering
def notify_render(renderingTimeList, notes = ""):
    title = "Render done"
    allNumber = len(renderingTimeList)
    renderedNumber = sum(x>0 for x in renderingTimeList)
    idx = [i for i,x in enumerate(renderingTimeList) if x>0]

    # calc average
    avg = 0
    for i in idx:
        avg += renderingTimeList[i]
    avg /= renderedNumber

    msg = "{} out of {} rendered, avg: {} [sec]".format(renderedNumber, allNumber, avg)
    if notes != "":
        msg += "\n[notes]\n{}".format(notes)

    # send notify
    notify(title, msg)
    return



####################

# 現在時刻を書き出す
def putTimeNow():
    dtNow = datetime.datetime.now()
    return str(dtNow.month) + "." + str(dtNow.day) + "_" + str(dtNow.hour) + ":" + str(dtNow.minute) + ":" + str(dtNow.second)

# 現在の日時に応じたディレクトリをつくる。
# ディレクトリ作成後、パスを返す
def mkdirToday():
    dtNow = datetime.datetime.now()
    dname = "../Output/" + str(dtNow.month).zfill(2) + str(dtNow.day).zfill(2)
    if not os.path.exists(dname):
        os.makedirs(dname)
    return dname

# ディレクトリ作成
def mkdir(directoryPath):
    if not os.path.exists(directoryPath):
        os.makedirs(directoryPath)
    return directoryPath

# 出力先が存在するかチェック
# …便利だと思ったけど一行で済んだ。
def outputFileCheck(filePath):
    return os.path.exists(filePath)

# 画像にテキスト書き込む
def writeText(filename, savename, text, size, mergin, color, thickness):
    img = cv.imread(filename)
    cv.putText(img, text, (mergin, size+mergin), cv.FONT_HERSHEY_PLAIN, color, thickness, cv.LINE_AA)
    cv.imwrite(savename)

def set_variant(variant):
    mi.set_variant(variant)

def Run(xmlfile, savepath, mysensor, deg, variant="scalar_rgb", gamma=True, UIntVal=16, saveEXR = False, overWrite=False, tmpexr = "./tmp.exr", renderingFilename='../scenes/_render.xml'):
    
    if saveEXR:
        dname = savepath[0: savepath.rfind("/")]
        fname = savepath[savepath.rfind("/")+1:]
        fname = fname[0: fname.rfind(".")]
        # print("fname: " + fname)
        # mkdir
        dname = mkdir(dname + "/" + "exr")
        tmpexr = dname + "/" + fname + ".exr"
        tmppng = dname + "/" + fname + ".png"
        tmp = dname + "/" + fname

    renderingFilename = xmlfile
    #mi.set_variant(variant)

    #sensors = [util.load_sensor(radius, phi, theta) for phi in phis]

    
    scene = mi.load_file(renderingFilename)
    #images = [mi.render(scene, spp=16, sensor=sensor) for sensor in sensors]
    image = mi.render(scene, sensor=mysensor)
    #print(tmpexr)
    plt.axis("off")
    plt.imshow(image ** (1.0 / 2.2)) # approximate sRGB tonemapping
        
    #mi.util.write_bitmap(tmppng, image)
    #mi.util.write_bitmap(tmpexr, image)
    mi.util.write_bitmap(tmp + str(deg) + ".png", image)
    mi.util.write_bitmap(tmp + str(deg) + ".exr", image)
    #mi.util.write_bitmap("result.png", image)

def Rough_Run(xmlfile, savepath, mysensor, deg, gl_rou, mi_rou, variant="scalar_rgb", gamma=True, UIntVal=16, saveEXR = False, overWrite=False, tmpexr = "./tmp.exr", renderingFilename='../scenes/_render.xml'):
    
    if saveEXR:
        dname = savepath[0: savepath.rfind("/")]
        fname = savepath[savepath.rfind("/")+1:]
        fname = fname[0: fname.rfind(".")]
        # print("fname: " + fname)
        # mkdir
        dname = mkdir(dname + "/" + "exr")
        #tmpexr = dname + "/" + fname + ".exr"
        #tmppng = dname + "/" + fname + ".png"
        tmp = dname + "/" + fname

    renderingFilename = xmlfile
    #mi.set_variant(variant)

    #sensors = [util.load_sensor(radius, phi, theta) for phi in phis]

    
    scene = mi.load_file(renderingFilename)
    #images = [mi.render(scene, spp=16, sensor=sensor) for sensor in sensors]
    image = mi.render(scene, sensor=mysensor)
    #print(tmpexr)
    plt.axis("off")
    plt.imshow(image ** (1.0 / 2.2)) # approximate sRGB tonemapping
        
    #mi.util.write_bitmap(tmppng, image)
    #mi.util.write_bitmap(tmpexr, image)
    mi.util.write_bitmap(tmp + '--deg-' + str(deg) + '--gl-' + str(gl_rou) + '--mi-' + str(mi_rou) + ".png", image)
    mi.util.write_bitmap(tmp + '--deg-' + str(deg) + '--gl-' + str(gl_rou) + '--mi-' + str(mi_rou) + ".exr", image)

def Multi_Rough_Run(xmlfile, savepath, cam_display_dis, deg, gl_rou, mi_rou, variant="scalar_rgb", gamma=True, UIntVal=16, saveEXR = False, overWrite=False, tmpexr = "./tmp.exr", renderingFilename='../scenes/_render.xml'):
    
    if saveEXR:
        dname = savepath[0: savepath.rfind("/")]
        fname = savepath[savepath.rfind("/")+1:]
        fname = fname[0: fname.rfind(".")]
        # print("fname: " + fname)
        # mkdir
        #quit()
        dname = mkdir(dname + "/" + "exr")
        #tmpexr = dname + "/" + fname + ".exr"
        #tmppng = dname + "/" + fname + ".png"
        tmp = dname + "/"# + fname

    renderingFilename = xmlfile

    scene = mi.load_file(renderingFilename)
    
    image = mi.render(scene)
    plt.axis("off")
    plt.imshow(image ** (1.0 / 2.2)) # approximate sRGB tonemapping
        
    #mi.util.write_bitmap(tmppng, image)
    #mi.util.write_bitmap(tmpexr, image)
    mi.util.write_bitmap(tmp + 'dis-' + str(cam_display_dis) + '--deg-' + str(deg) + '--gl-' + str(gl_rou) + '--mi-' + str(mi_rou) + ".png", image)
    mi.util.write_bitmap(tmp + 'dis-' + str(cam_display_dis) + '--deg-' + str(deg) + '--gl-' + str(gl_rou) + '--mi-' + str(mi_rou) + ".exr", image)


def MultiRun(xmlfile, savepath, variant="scalar_rgb", gamma=True, UIntVal=16, saveEXR = False, overWrite=False, tmpexr = "./tmp.exr", renderingFilename='../scenes/_render.xml'):
    
    if saveEXR:
        dname = savepath[0: savepath.rfind("/")]
        fname = savepath[savepath.rfind("/")+1:]
        fname = fname[0: fname.rfind(".")]
        # print("fname: " + fname)
        # mkdir
        dname = mkdir(dname + "/" + "exr")
        tmpexr = dname + "/" + fname + ".exr"
        tmppng = dname + "/" + fname + ".png"
        tmp = dname + "/" + fname

    renderingFilename = xmlfile
    mi.set_variant(variant)

    sensor_count = 4

    radius = 10
    phis = [20.0 * i for i in range(sensor_count)]
    theta = 60.0


    sensors = [util.load_sensor(radius, phi, theta) for phi in phis]
    print(sensors[0])
    scene = mi.load_file(renderingFilename)
    images = [mi.render(scene, spp=16, sensor=sensor) for sensor in sensors]
    #image = mi.render(scene, spp=256, sensor=sens)
    #print(tmpexr)
    for i, image in enumerate(images):
        plt.axis("off")
        plt.imshow(image ** (1.0 / 2.2)) # approximate sRGB tonemapping
        
        mi.util.write_bitmap(tmppng, image)
        mi.util.write_bitmap(tmpexr, image)
        mi.util.write_bitmap(tmp + str(i) + ".png", image)
        mi.util.write_bitmap(tmp + str(i) + ".exr", image)
    #mi.util.write_bitmap("result.png", image)




# Run Mitsuba Render, Save as BMP-type fileformat
def runRender(xmlfile, savepath, variant="gpu_rgb", gamma = True, UIntVal = 16, saveEXR = False, overWrite=False, tmpexr = "./tmp.exr", renderingFilename = '../XML/_render.xml'):
    # existsCheck
    if overWrite==False:
        if os.path.exists(savepath):
            return
    runningTimes = []    
    start = time.time()                     # tic

    # saveEXR==True -> change `tmpexr` var
    if saveEXR:
        dname = savepath[0: savepath.rfind("/")]
        fname = savepath[savepath.rfind("/")+1:]
        fname = fname[0: fname.rfind(".")]
        # print("fname: " + fname)
        # mkdir
        dname = mkdir(dname + "/" + "exr")
        tmpexr = dname + "/" + fname + ".exr"
        # print("dname: " + dname)
        # print("ename: " + tmpexr)
    # del pyvarvar

    xmlManip.delPyvars(xmlfile, renderingFilename)

    runningTimes.append(time.time()-start)  # toc
    start = time.time()                     # tic

    mitsuba.set_variant(variant)
    from mitsuba import Thread, math, Properties, Frame3f, Float, Vector3f, warp, Bitmap, Struct
    from mitsuba import load_file, load_string
    from mitsuba import BSDF, BSDFContext, BSDFFlags, BSDFSample3f, SurfaceInteraction3f, \
                            register_bsdf, Texture, MicrofacetDistribution, reflect, MicrofacetType

    runningTimes.append(time.time()-start)  # toc
    start = time.time()                     # tic

    Thread.thread().file_resolver().append(os.path.dirname(renderingFilename))

    runningTimes.append(time.time()-start)  # toc
    start = time.time()                     # tic

    scene = load_file(renderingFilename)

    runningTimes.append(time.time()-start)  # toc
    start = time.time()                     # tic

    scene.integrator().render(scene, scene.sensors()[0])

    runningTimes.append(time.time()-start)  # toc
    start = time.time()                     # tic


    film = scene.sensors()[0].film()
    #film.setDestinationFile(tmpexr)

    runningTimes.append(time.time()-start)  # toc
    start = time.time()                     # tic

    film.develop()

    runningTimes.append(time.time()-start)  # toc
    start = time.time()                     # tic

    bmp = film.bitmap(raw=True)

    runningTimes.append(time.time()-start)  # toc
    start = time.time()                     # tic


    sttype = Struct.Type.UInt8
    if UIntVal==16:
        sttype = Struct.Type.UInt16

    runningTimes.append(time.time()-start)  # toc
    start = time.time()                     # tic

    bmp.convert(Bitmap.PixelFormat.RGB, sttype, srgb_gamma=gamma).write(savepath)

    runningTimes.append(time.time()-start)  # toc
    ## show times
    print("="*40)
    print("delPyvar:\t\t" + str(runningTimes[0]))
    print("setVariant:\t\t" + str(runningTimes[1]))
    print("thread.thread:\t\t" + str(runningTimes[2]))
    print("load_file:\t\t" + str(runningTimes[3]))
    print("integrator.render:\t" + str(runningTimes[4]))
    print("film:\t\t\t" + str(runningTimes[5]))
    print("filmDev:\t\t" + str(runningTimes[6]))
    print("filmBmp:\t\t" + str(runningTimes[7]))
    print("stTime:\t\t\t" + str(runningTimes[8]))
    print("bmp.convert:\t\t" + str(runningTimes[9]))
    print("="*40)
    # array = np.array(bmp)
    # print("\n\n[num]\tmaxes\taverages(raw data)\n")
    # for i in range(array.shape[2]):
    #     print("[{}]\t".format(str(i)),np.amax(array[:,:,i]), "\t", np.average(array[:,:,i]))
    # print("\n=====================\n\n")
    # converted = bmp.convert(Bitmap.PixelFormat.RGB, sttype, srgb_gamma=False)
    # print(converted.shape)
    # print("\n\n[num]\tmaxes\taverages(converted data)\n")
    # for i in range(converted.shape[2]):
    #     print("[{}]\t".format(str(i)),np.amax(converted[:,:,i]), "\t", np.average(converted[:,:,i]))
    # print("\n=====================\n\n")


# extract positive or negative elements in numpy.array
def extractP(array):
    positives = np.copy(array)
    print(positives.shape)
    positives = np.where(positives>0, positives, 0)
    return positives

def load_sensor(origin):
    # Apply two rotations to convert from spherical coordinates to world 3D coordinates.
    from mitsuba import ScalarTransform4f as T
    #origin = T.rotate([0, 0, 1], phi).rotate([0, 1, 0], theta) @ mi.ScalarPoint3f([0, 0, r])
    #print(origin)
    #quit()
    #origin = [-0.5, 0, -80]

    return mi.load_dict({
        'type': 'perspective',
        #'fov_axis': 'smaller',
        'near_clip': 10,
        'far_clip' : 2800,
        #'focus_distance': 1000,
        'focal_length': "35",
        #'fov': 70,
        'to_world': T.look_at(
            origin=origin,
            target=[0, 0, -30],
            up=[0, 1, 0]
        ),
        'sampler': {
            'type': 'independent',
            'sample_count': 1024
        },
        'film': {
            'type': 'hdrfilm',
            'width': 1080, #2160,
            'height': 1080, #1440,
            'rfilter': {
                'type': 'gaussian',
            },
            'pixel_format': 'rgb',
        },
    })

def extractN(array):
    negatives = np.copy(array)
    negatives = np.where(negatives<0, negatives, 0)
    return negatives

def imNorm(a, b):
    # Normalize array A by deviding B
    # /0 div => replace 0
    c = a / b
    c = np.nan_to_num(c)
    # for i in range(a.shape[0]):
    #     for j in range(a.shape[1]):
    #         for k in range(a.shape[2]):
    #             c[i][j][k] = c[i][j][k] if c[i][j][k]!= np.nan else 0   # 0/0 => 0
    #             c[i][j][k] = c[i][j][k] if c[i][j][k]!= np.inf else 0   # xxx/0 => 0
    return c

# Run Mitsuba Render, Save as Stokes fileformat
def runRenderWithStokes(xmlfile, savepath, variant="scalar_spectrum_polarized", gamma = True, UIntVal = 16, saveEXR = False, overWrite=False, tmpexr = "./tmp.exr", renderingFilename = '../XML/_render.xml'):
    # existsCheck
    if overWrite==False:
        if os.path.exists(savepath):
            return
    # saveEXR==True -> change `tmpexr` var
    if saveEXR:
        dname = savepath[0: savepath.rfind("/")]
        fname = savepath[savepath.rfind("/")+1:]
        fname = fname[0: fname.rfind(".")]
        # print("fname: " + fname)
        # mkdir
        dname = mkdir(dname + "/" + "exr")
        tmpexr = dname + "/" + fname + ".exr"
        # print("dname: " + dname)
        # print("ename: " + tmpexr)
    # del pyvarvar

    xmlManip.delPyvars(xmlfile, renderingFilename)

    mitsuba.set_variant(variant)
    from mitsuba.core import Thread, math, Properties, Frame3f, Float, Vector3f, warp, Bitmap, Struct
    from mitsuba.core.xml import load_file, load_string
    from mitsuba.render import BSDF, BSDFContext, BSDFFlags, BSDFSample3f, SurfaceInteraction3f, \
                            register_bsdf, Texture, MicrofacetDistribution, reflect, MicrofacetType

    Thread.thread().file_resolver().append(os.path.dirname(renderingFilename))
    scene = load_file(renderingFilename)
    scene.integrator().render(scene, scene.sensors()[0])
    film = scene.sensors()[0].film()
    #film.setDestinationFile(tmpexr)
    film.develop()
    bmp = film.bitmap(raw=True)

    # exr to RGB stokes
    # matplotlib doesnt support 16bit images
    array = np.array(bmp)
    # Channels 0--4 : RGBA, RGB, oppacity, and... something
    rgba = array[:, :, :5]

    # Channels 5--7 : s0, wittern as RGB
    s0 = array[:, :, 5:8]

    # Channels 8--10 : s1
    s1 = array[:, :, 8:11]
    
    # Channels 11--13 : s2
    s2 = array[:, :, 11:14]
    
    # Channels 14--16 : s3
    s3 = array[:, :, 14:]

    # normalize s1 -- s3 using s0 value
    s1 = imNorm(s1, s0)
    s2 = imNorm(s2, s0)
    s3 = imNorm(s3, s0)

    # convert s1--s3 from mono to R <=> G
    # s0modified = np.zeros_like(s0)
    # s0modified[:,:,0] = extractP(s0)[:,:,0]
    # s0modified[:,:,1] = extractN(s0)[:,:,0]*-1
    # s1modified = np.zeros_like(s1)
    # s1modified[:,:,0] = extractP(s1)[:,:,0]
    # s1modified[:,:,1] = extractN(s1)[:,:,0]*-1

    print("\n\n[num]\tmin\tmax\tavg\n")
    for i in range(array.shape[2]):
        print("[{}]\t".format(str(i)),np.amin(array[:,:,i]),"\t",np.amax(array[:,:,i]), "\t", np.average(array[:,:,i]))
    print("\n=====================\n\n")

    
    sv = savepath[0: savepath.rfind(".")]   #save
    ext = savepath[savepath.rfind("."):]  #.ext
    # print(array)
    # print("\n============================\n")
    # print("shape:", array.shape)
    # for i in range(array.shape[2]):
    #     print(np.average(array[:,:,i]))
    
    # imSaves
    # savePath + "-{}" + extension
    sttype = Struct.Type.UInt8
    if UIntVal==16:
        sttype = Struct.Type.UInt16
    # print(bmp)
    # bmp.convert(Bitmap.PixelFormat.MultiChannel, sttype, srgb_gamma=gamma).write(sv+"-rgba", format=Bitmap.FileFormat.Auto)
    # plt.imsave(sv+"-s0"+ext, np.clip(s0, 0, 1))
    # plt.imsave(sv+"-s0"+ext, s0[:,:,0])
    # plt.imsave(sv+"-s1"+ext, s1[:,:,0], cmap="coolwarm", vmin=-0.01, vmax=+0.01)
    # plt.imsave(sv+"-s1"+ext, s1, cmap="RdYlGn", vmin=-0.01, vmax=+0.01)
    print("\n\n[num]\tmin\tmax\tavg\t(normalized)\n")
    print("{}\t{}\t{}".format(np.amin(s0), np.amax(s0), np.average(s0)))
    print("{}\t{}\t{}".format(np.amin(s1), np.amax(s1), np.average(s1)))
    print("{}\t{}\t{}".format(np.amin(s2), np.amax(s2), np.average(s2)))
    print("{}\t{}\t{}".format(np.amin(s3), np.amax(s3), np.average(s3)))
    print("\n=====================\n\n")
    plt.imsave(sv+"-s1"+ext, s1[:,:,0], cmap="RdYlGn", vmin=-1, vmax=1)
    plt.imsave(sv+"-s2"+ext, s2[:,:,0], cmap="RdYlGn", vmin=-1, vmax=1)
    plt.imsave(sv+"-s3"+ext, s3[:,:,0], cmap="RdYlGn", vmin=-1, vmax=1)


