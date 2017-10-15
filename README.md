# ClassicAL-DocFx-Plugin

DocFX Plugin and object metadata exporter.

Visual studio solution in this repository contains 2 project
- ClassicDynamicsNavDocExporter - executable that can generate object metadata for DocFX from Dynamics Nav source code text files
- ClassicDynamicsNavReference - library used by both ClassicDynamicsNavDocExporter and DocFX plugin to process objects metadata

Full DocFX template can be found in ClassicAL-DocFx-Template template, it already contains dll produced by ClassicDynamicsNavReference in docfx_project\defaultcal\plugins folder

If you want to change something in ClassicDynamicsNavReference, make sure that you also copy this dll to your docfx_project\defaultcal\plugins folder

How to use it
1. Download empty DocFX project from my ClassicAL-DocFx-Template repository
1. Export your Dynamics Nav objects to text file to docfx_project\src folder of project downloaded in step 1
2. Run ClassicDynamicsNavDocExporter.exe and specify Dynamics Nav source code text file, output path for object metadata (docfx_project\api) and name of your project (it will be used in generated documentation). Assuming that you extracted docfx_project from step 1 in c:\temp\docfx_project, your project name is MyDynamicsNavProject and your objects are in AllObjects.txt file, you shour run this command:
    ClassicDynamicsNavDocExporter.exe c:\temp\docfx_project\src\AllObjects.txt c:\temp\docfx_project\api MyDynamicsNavProject
3. Go to your docfx_project folder and run docfx to build documentation from the metadata:
    cd c:\temp\docfx_project
    docfx
4. Your documentation should be generated in docfx_project\_site\api folder, go there and open index.html file to check if it looks fine. It might not work for you this way, because this template is based on standard c# docfx template and uses plenty of javascript. I was able to open index.html file directly in firefox, it does not work this way in opera, chrome and IE for me. It works fine if you publish whole _site folder on IIS.

    

    



