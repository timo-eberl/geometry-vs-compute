# Geometry Shader vs GPU Instancing Comparison

A performance comparison of drawing thousands of objects in Unity with

1. GPU Instancing
2. A Geometry Shader

Inspired by Jasper Flick's [Compute Shaders Tutorial](https://catlikecoding.com/unity/tutorials/basics/compute-shaders/)

## Performance tests

Test conditions: 1920x1080, 1000 graph resolution (1 Mio Qubes which equals roughly 24 Mio Verts)

IN = GPU Instancing, GS = Geometry Shader

- Desktop PC 1
  - dGPU (AMD Radeon RX 6700 XT)
    - bare metal Windows: IN 300 FPS, GS 72 FPS
    - bare metal Linux: IN 150 FPS, GS 130 FPS
    - VM Windows with GPU Passthrough: IN 500 FPS, GS 163 FPS
  - iGPU (AMD Ryzen 5 5600G)
    - bare metal Linux: IN 55 FPS, GS 18 FPS
    - bare metal Windows: IN 33 FPS, GS 11 FPS
- Desktop PC 2 (GTX 970 or GTX 960 idk anymore)
  - bare metal Windows: IN 110 FPS, GS 200 FPS
- Desktop PC 3 (AMD Radeon RX 6750 XT)
  - bare metal Windows: IN 420 FPS, GS 135 FPS
- Desktop PC 4 (Nvidia RTX 3080)
  - bare metal Windows: IN 152 FPS, GS 600 FPS
- Desktop PC 5 (Nvidia RTX 3080)
  - bare metal Windows: IN 138 FPS, GS 510 FPS
- Laptop HP EliteBook 865 G9, Ryzen 7 PRO 6850U (iGPU)
  - bare metal Linux: IN 60 FPS, GS 45 FPS
