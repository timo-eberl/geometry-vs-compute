# Geometry Shader vs Compute Shader Comparison

A performance comparison of drawing thousands of objects with a compute shader setup and a geometry shader setup in Unity.

Inspired by Jasper Flick's [Compute Shaders Tutorial](https://catlikecoding.com/unity/tutorials/basics/compute-shaders/)

## Performance tests so far

Test conditions: 1920x1080, 1000 graph resolution (1 Mio Qubes which equals roughly 24 Mio Verts)

- Desktop PC 1
  - dGPU (AMD Radeon RX 6700 XT)
    - bare metal Windows: CS 300 FPS, GS 72 FPS
    - bare metal Linux: CS 150 FPS, GS 130 FPS
    - VM Windows with GPU Passthrough: CS 500 FPS, GS 163 FPS
  - iGPU (AMD Ryzen 5 5600G)
    - bare metal Linux: CS 55 FPS, GS 18 FPS
    - bare metal Windows: CS 33 FPS, GS 11 FPS
- Desktop PC 2 (GTX 970 or GTX 960 idk anymore)
  - bare metal Windows: CS 110 FPS, GS 200 FPS
- Laptop HP EliteBook 865 G9, Ryzen 7 PRO 6850U (iGPU)
  - bare metal Linux: CS 60 FPS, GS 45 FPS
