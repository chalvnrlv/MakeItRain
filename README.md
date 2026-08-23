# 🌧️ Make It Rain — Interactive Audio-Visual Experience

An interactive audiovisual installation and digital playground built in **Unity** powered by **OpenCVSharp**. 

*Make It Rain* captures real-time webcam input, extracts silhouettes/contours using computer vision, and transforms physical silhouettes into dynamic colliders. Falling particles interact physically with your silhouette and each other, triggering randomized neon glows, bloom effects, and dynamic audio-reactive spawning.

---

## 📸 Demo & Preview

![Make It Rain Preview]

[![Watch Demo Video]
> 📹 *Click above to watch the video demonstration on YouTube!*

---

## ✨ Features

- **Real-Time Silhouette Detection:** Uses OpenCV to threshold and extract contours from the live webcam feed, projecting them into dynamic `PolygonCollider2D` shapes.
- **Physical Collision & Neon Glow VFX:** Particles bounce off silhouettes and each other. Contacts trigger custom HDR emission flashes with smooth bloom fades (`ParticleGlow.cs`).
- **Dynamic Palette Randomization:** Each recycled particle spawns with a vibrant randomized HSV color palette, matching its base material to its glowing impact reaction.
- **Audio-Reactive Particle Spawning:** Measures real-time microphone RMS loudness to scale rain frequency and particle size during voice or musical beats (`MicInput.cs`).
- **Optimized Performance:** Implements object pooling (`Emitter.cs`) and auto-cleanup destroy zones (`DestroyZone.cs`) for smooth frame rates.

---

## 🛠️ Built With

- **Engine:** [Unity](https://unity.com/) (Universal Render Pipeline / URP)
- **Computer Vision:** [OpenCVSharp](https://github.com/shimat/opencvsharp)
- **Language:** C#
- **Rendering:** URP Post-Processing (Bloom, HDR Emission)

---

## 🚀 Getting Started

### Prerequisites
- Unity **2022.3 LTS** or newer (with URP support).
- A working **Webcam** and **Microphone** (optional for audio reactivity).

### Installation & Setup

1. **Clone the repository:**
   ```bash
   git clone https://github.com/chalvnrlv/MakeItRain.git
   cd MakeItRain
   ```

2. **Open in Unity Hub:**
   - Launch Unity Hub.
   - Click **Add** and select the cloned folder `MakeItRain`.
   - Open the project.

3. **Open the Scene:**
   - Navigate to `Assets/Scenes/Main.unity` (or `SampleScene.unity`) and open it.

4. **Run the Project:**
   - Ensure your webcam is connected and enabled.
   - Hit **Play** ▶️ in Unity.
   - Stand in front of your camera and watch the rain react to your silhouette!

---

## ⚙️ Key Configuration & Inspector Settings

### 1. `Finder.cs` (Webcam & Contour Detection)
- **`Threshold`**: Adjust depending on your background contrast / lighting.
- **`CurveAccuracy`**: Simplification level for polygon collider calculation.
- **`MinArea`**: Filters out small noise specks from becoming colliders.

### 2. `Emitter.cs` (Particle Rain & Spawner)
- **`Use Mic Input`**: Toggle to enable/disable microphone reactivity.
- **`Max / Min Spawn Rate`**: Controls precipitation density range.
- **`Size Boost`**: Dictates how much audio loudness swells particle scale.

### 3. `ParticleGlow.cs` (Visuals & Collision)
- **`Hue / Sat / Val Min-Max`**: Tune randomized color ranges (supports vibrant rainbow or monochromatic styles).
- **`Max Intensity`**: Peak HDR brightness for bloom triggers.
- **`Rise / Hold / Fade Time`**: Timing curves for the collision glow pulse.

---

## 📂 Project Structure

```
Assets/
├── Scripts/
│   ├── Finder.cs           # Webcam capture, OpenCV thresholding & collider setup
│   ├── Emitter.cs          # Object pool & spawn rate controller
│   ├── ParticleGlow.cs     # Collision detection, randomized HSV & HDR emission
│   ├── MicInput.cs         # Real-time microphone RMS loudness analysis
│   └── DestroyZone.cs      # Boundary trigger to recycle particles
├── Prefabs/
│   └── Sphere.prefab       # Physical particle prefab with collider & glow
├── Scenes/
│   └── Main.unity          # Main interactive scene
└── OpenCV+Unity/            # Native OpenCV plugins and demo assets
```

---

## 👤 Author

- **Chalvin** - [GitHub Profile](https://github.com/chalvnrlv)
