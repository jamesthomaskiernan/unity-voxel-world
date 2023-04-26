## About

This a voxel world prototype implemented in the Unity game engine. The world is rendered with an indirect draw call, meaning the voxel meshes are generated on the fly in a compute shader and then sent to the vertex shader in a structured buffer. This allows the entire world to be generated near instantly, which enables cool features like z scrolling through the world's layers. Voxels can be both cubes, or custom meshes specified by the user. Grass is also generated in a compute shader and placed on surface grass voxels.

## Controls

- WASD keys to move camera
- = and - keys to zoom camera
- IJKL keys to move point light

## Performance

On a RTX 3090 at a resolution of 3840 x 2160, this implementations runs at ~450 fps with grass enabled, and ~600 fps with grass disabled.

## Screenshots

<p>Smaller world</p>
<img src="./Screenshots/world_small.png" width="1066" height="600px">

<p>Instant z scrolling through world layers</p>
<img src="./Screenshots/scroll.gif" width="1066" height="600px">

<p>Grass shader and custom voxel type</p>
<img src="./Screenshots/grass.gif" width="1066" height="600px">

<p>Larger world sizes</p>
<img src="./Screenshots/world_large.png" width="1066" height="600px">