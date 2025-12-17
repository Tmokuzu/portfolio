import os
import sys

# pyvar: Python Variable, Python側で記述する変数(勝手に作った)
# XMLファイル側で、変数を指定したいところの末尾にpyvar="変数名"といった感じに記述して使う。

# (xml側でこのように書いたら)
# <shape type="sphere">
#   <float name="radius" value="XXX" pyvar="variableSample"/>
# </shape>

# (Python側で)
# changeVal(filepath, "variableSample", YYY)
# こういう関数を呼び出すと、xml側のvalueがYYYに設定される。

# 指定されたpyvarのvalueを書き換える関数。
# 1行に「pyvar="XX"」がひとつだけ含まれていること、
# xmlファイルがキレイに書かれていること(1行に複数個タグが書かれていないこと)が条件。
def changeVal(filepath, outputfilePath, varName, value):
    with open(filepath, mode="r", encoding="utf-8") as file:
        filelines = file.readlines()
    file.close()

    for i in range(len(filelines)):
        # 指定したvarNameがみつかるか
        scanningLine = filelines[i]
        if(scanningLine.find('pyvar="' + varName) > -1):
            # 見つかった：valueを置き換える
            # 「value="」は7文字あるのでその文進めて置き換えるインデックスを設定
            replacedLine = scanningLine[0:scanningLine.find('value="') + 7]
            replacedLine = replacedLine + str(value) + '" pyvar="' + str(varName) + '"/>\n'
            print(filelines[i] + " => " + replacedLine) # debug
            filelines[i] = filelines[i].replace(filelines[i], replacedLine)

    with open(outputfilePath, mode="w", encoding="utf-8") as file:
        file.writelines(filelines)
    file.close()

# デバッグ
"""
filepath = "./xmlManipulatorTest.xml"
ofilepath = "./xmlManipulatorTest_result.xml"
varName = "testID"
value = "XXX"
changeVal(filepath, ofilepath, varName, value)
"""

# 指定されたpyvarのtranslateを書き換える。
# changeValのtranslate版。
def changeTranslate(filepath, outputfilePath, varName, position=[0, 0, 0]):
    with open(filepath, mode="r", encoding="utf-8") as file:
        filelines = file.readlines()
    file.close()

    for i in range(len(filelines)):
        # 指定したvarNameがみつかるか
        scanningLine = filelines[i]
        if(scanningLine.find('pyvar="' + varName) > -1):
            # 見つかった：translateを追加する。
            # 「translate 」は10文字あるのでその分進めて置き換えるインデックスを設定
            replacedLine = scanningLine[0:scanningLine.find('translate ') + 10]
            replacedLine = replacedLine + 'x="{}" y="{}" z="{}"'.format(*position) + ' pyvar="' + str(varName) + '"/>\n'
            print(filelines[i] + " => " + replacedLine) # debug
            filelines[i] = filelines[i].replace(filelines[i], replacedLine)

    with open(outputfilePath, mode="w", encoding="utf-8") as file:
        file.writelines(filelines)
    file.close()

# デバッグ
"""
filepath = "./xmlManipulatorTest.xml"
varName = "test_translateID"
pos = [100, 200, 300]
changeTranslate(filepath, ofilepath, varName, pos)
"""

# 指定されたpyvarのscaleを書き換える。
def changeScale(filepath, outputfilePath, varName, scale=[0, 0, 0]):
    with open(filepath, mode="r", encoding="utf-8") as file:
        filelines = file.readlines()
    file.close()

    for i in range(len(filelines)):
        # 指定したvarNameがみつかるか
        scanningLine = filelines[i]
        if(scanningLine.find('pyvar="' + varName) > -1):
            # 見つかった：scaleを追加する。
            # 「scale 」は6文字あるのでその分進めて置き換えるインデックスを設定
            replacedLine = scanningLine[0:scanningLine.find('scale ') + 6]
            replacedLine = replacedLine + 'x="{}" y="{}" z="{}"'.format(*scale) + ' pyvar="' + str(varName) + '"/>\n'
            print(filelines[i] + " => " + replacedLine) # debug
            filelines[i] = filelines[i].replace(filelines[i], replacedLine)

    with open(outputfilePath, mode="w", encoding="utf-8") as file:
        file.writelines(filelines)
    file.close()

# 指定されたpyvarのspectrum(arg: only one)を書き換える。
def changeSpectrum01(filepath, outputfilePath, varName, value):
    with open(filepath, mode="r", encoding="utf-8") as file:
        filelines = file.readlines()
    file.close()

    for i in range(len(filelines)):
        # 指定したvarNameがみつかるか
        scanningLine = filelines[i]
        if(scanningLine.find('pyvar="' + varName) > -1):
            # 見つかった：valueを置き換える
            # 「value="」は7文字あるのでその文進めて置き換えるインデックスを設定
            replacedLine = scanningLine[0:scanningLine.find('value="') + 7]
            replacedLine = replacedLine + str(value) + '" pyvar="' + str(varName) + '"/>\n'
            print(filelines[i] + " => " + replacedLine) # debug
            filelines[i] = filelines[i].replace(filelines[i], replacedLine)

    with open(outputfilePath, mode="w", encoding="utf-8") as file:
        file.writelines(filelines)
    file.close()

# 指定されたpyvarのlookatを書き換える。
# changeValのlookat版。
def changeLookat(filepath, outputfilePath, varName, origin=[0, 0, 0], target=[0, 0, 0], up=[0, 1, 0]):
    with open(filepath, mode="r", encoding="utf-8") as file:
        filelines = file.readlines()
    file.close()

    for i in range(len(filelines)):
        # 指定したvarNameがみつかるか
        scanningLine = filelines[i]
        if(scanningLine.find('pyvar="' + varName) > -1):
            # 見つかった：lookatを書き換える。
            # 「lookat 」は7文字あるのでその分進めて置き換えるインデックスを設定
            replacedLine = scanningLine[0:scanningLine.find('lookat ') + 7]
            replacedLine = replacedLine + 'origin="{}, {}, {}"'.format(*origin) + ' target="{}, {}, {}"'.format(*target) + ' up="{}, {}, {}"'.format(*up) +' pyvar="{}"/>\n'.format(varName)
            print(filelines[i] + " => " + replacedLine) # debug
            filelines[i] = filelines[i].replace(filelines[i], replacedLine)

    with open(outputfilePath, mode="w", encoding="utf-8") as file:
        file.writelines(filelines)
    file.close()

# 指定されたpyvarのscaleを書き換える。
# changeValのscale版。
def changeScale(filepath, outputfilePath, varName, scale=[0, 0, 0]):
    with open(filepath, mode="r", encoding="utf-8") as file:
        filelines = file.readlines()
    file.close()

    for i in range(len(filelines)):
        # 指定したvarNameがみつかるか
        scanningLine = filelines[i]
        if(scanningLine.find('pyvar="' + varName) > -1):
            # 見つかった：scaleを書き換える。
            # 「scale 」は6文字あるのでその分進めて置き換えるインデックスを設定
            replacedLine = scanningLine[0:scanningLine.find('scale ') + 6]
            replacedLine = replacedLine + 'x="{}" y="{}" z="{}"'.format(*scale) + ' pyvar="{}"/>\n'.format(varName)
            print(filelines[i] + " => " + replacedLine) # debug
            filelines[i] = filelines[i].replace(filelines[i], replacedLine)

    with open(outputfilePath, mode="w", encoding="utf-8") as file:
        file.writelines(filelines)
    file.close()

# 指定されたpyvarのlookatを書き換える。
# changeValのlookat版。
def changeRotate(filepath, outputfilePath, varName, rotateAxis, angle):
    with open(filepath, mode="r", encoding="utf-8") as file:
        filelines = file.readlines()
    file.close()

    for i in range(len(filelines)):
        # 指定したvarNameがみつかるか
        scanningLine = filelines[i]
        if(scanningLine.find('pyvar="' + varName) > -1):
            # 見つかった：書き換える。
            # 「rotate 」は7文字あるのでその分進めて置き換えるインデックスを設定
            replacedLine = scanningLine[0:scanningLine.find('rotate ') + 7]
            replacedLine = replacedLine + str(rotateAxis) + '="1" angle="{}"'.format(angle) + ' pyvar="{}"/>\n'.format(varName)
            print(filelines[i] + " => " + replacedLine) # debug
            filelines[i] = filelines[i].replace(filelines[i], replacedLine)

    with open(outputfilePath, mode="w", encoding="utf-8") as file:
        file.writelines(filelines)
    file.close()


# そのままだとレンダリングできないのでpyvar="XXX"を全消しする。
def delPyvars(filepath, outputfilepath):
    with open(filepath, mode="r", encoding="utf-8") as file:
        filelines = file.readlines()
    file.close()

    for i in range(len(filelines)):
        # pyvar探す
        scanningLine = filelines[i]
        if(scanningLine.find("pyvar=") > -1):
            # 見つかった：replace
            findvarString = scanningLine[scanningLine.find(" pyvar="):scanningLine.rfind('"')+1]

            print("delete: " + findvarString) # debug
            filelines[i] = filelines[i].replace(findvarString, '')

    with open(outputfilepath, mode="w", encoding="utf-8") as file:
        file.writelines(filelines)
    file.close()

# resultfilepath = "./xmlManipulatorTest_result_del.xml"
# delPyvars(ofilepath, resultfilepath)