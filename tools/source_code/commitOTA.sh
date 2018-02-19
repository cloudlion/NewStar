#!/bin/sh

#example ./androidAutoBuild.sh 1.0.0 debug/release
# projectPath="/Users/cary/Documents/WorkAndroidProject/adam/client"
# projectPath="/Users/hliang/Adam/adam_android/client"
projectPath=$(pwd)/../../  #"/Users/linkage/androidProject/adam/client"
unityRootPath="${projectPath}/unity"
copyDestAssetBundlePath="${unityRootPath}/Assets/StreamingAssets/res"
copySourceAssetBundlePath="${unityRootPath}/Assets/AssetBundles/android/stream"
unityAppPath="/Applications/Unity/Unity.app/Contents/MacOS/Unity"
set -e
main()
{
    oldPath=$(pwd)
	cd $1
    git add -A
    git commit -m '打包提交ota'
    git push
    cd $oldPath
}

main $1
