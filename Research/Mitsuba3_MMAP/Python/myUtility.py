import cv2 as cv
import os 
import numpy as np 
import matplotlib.pyplot as plt
import drjit as dr
import mitsuba as mi
import datetime
import math as mathpy
import random
import myUtility as util
import time
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
        'fov_axis': 'smaller',
        'near_clip': 10,
        'far_clip' : 2800,
        'focus_distance': 1000,
        'fov': 70,
        'to_world': T.look_at(
            origin=origin,
            target=[0, 0, -79],
            up=[0, 1, 0]
        ),
        'sampler': {
            'type': 'independent',
            'sample_count': 1024
        },
        'film': {
            'type': 'hdrfilm',
            'width': 1024,
            'height': 1024,
            'rfilter': {
                'type': 'gaussian',
            },
            'pixel_format': 'rgb',
        },
    })
