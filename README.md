IndorilConverter
================

Tool to port TES3: Morrowind .esp/.esm files to TES5: Skyrim. Used for moving user-made mods to the newer engine, with some enhancements.

This is currently under heavy development and is NOT complete.


Current Status As Of 9 Dec 2014:
--------------------------------
Following Records are converted and functional:

LTEX (Land Textures)
LAND (Landscape - via TESAnnwyn. Added functionality of manual texture blending to remove seams and boxes.)
CELL (Interior/Exterior Cell Meta-Data like Names, Lighting, Water-Height etc.) 
LIGH (Lights. NOTE: NifConvert can add flame nodes where needed.)
STAT (Statics. NifConvert can convert all Static models.)
DOOR (Doors, including Animated Opening/Closing Versions)
REFR (Placed Objects in Cells, both Interior and Exterior. Including working portal doors.)

Water Meshes are placed in interiors to properly imitate morrowind interior water.


Future Features under development:
---------------------------------
People. Faces.
AI Packages (generation).
Activators