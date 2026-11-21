# Introduction to Augmented Reality – Assignment 4

**Author:** Jinzhou Cheng

This repository contains my solutions to the programming parts of Assignment 4:

- **Q1:** a 1D Kalman filter that estimates position and velocity (Python)
- **Q2:** an extended *Roll-a-Ball* game built in Unity

Written answers were submitted separately as a PDF on Gradescope.

---

## Repository structure

```
Assignment4/
├── README.md
├── Q1/
│   ├── e.py      # Kalman filter with the completed measurement-update step
│   └── f.py      # Same simulation with prediction only (no measurement update)
└── Q2/           # Unity project (Roll-a-Ball extension)
    ├── Assets/
    │   ├── Scenes/SampleScene.unity     # main game scene
    │   ├── PlayerController.cs          # movement, jump, pickups, power-up, wall, goal
    │   ├── CameraController.cs          # third-person camera that follows the player
    │   ├── Rotator.cs                   # spins the collectibles
    │   ├── InputSystem_Actions.inputactions
    │   └── *.mat                        # Gold (pickups), WallMat, GoalMat
    ├── Packages/
    ├── ProjectSettings/
    └── .gitignore                       # standard Unity .gitignore
```

---

## Q1: 1D Kalman filter (Python)

A simulated object moves in 1D under constant acceleration (`a = 2.0`, `dt = 0.1`, 50 steps). Its position is measured with Gaussian noise (σ = 2.0), and a Kalman filter estimates its state `[position, velocity]`.

| File | Description |
|------|-------------|
| `Q1/e.py` | The full Kalman filter: **prediction** (`x_pred = A x + B a`, `P_pred = A P Aᵀ + Q`) followed by the **measurement update** (innovation `y`, innovation covariance `S`, Kalman gain `K`, corrected state `x_est`, and corrected covariance `P`). |
| `Q1/f.py` | The same simulation with the measurement update removed (`x_est = x_pred`), so the estimate comes from the motion model alone. Comparing its plot with `e.py` shows how the estimate drifts away from the true position without measurement correction. |

### Requirements
- Python 3.8+
- `numpy` and `matplotlib`

```bash
pip install numpy matplotlib
```

### How to run
```bash
cd Q1
python e.py    # Kalman filter with measurement update
python f.py    # prediction only
```
Each script opens a plot of the **true position** (green line), **noisy measurements** (blue stars), and **Kalman filter estimate** (red dots) over time. The noise is random, so each run looks slightly different.

---

## Q2: Extended Roll-a-Ball (Unity)

The game builds on the Unity *Roll-a-Ball* tutorial and adds the following features:

| Part | Feature | Implementation |
|------|---------|----------------|
| (a) | **Follow camera** | `CameraController.cs` stores the initial camera-to-player offset and updates the camera position in `LateUpdate()`. |
| (b) | **Collectibles and counter** | 10 rotating `PickUp` cubes. Collecting one deactivates it and updates `CountText` ("Count: x / 10"). |
| (c) | **Start button** | Gravity is off and the ball can't move until the player clicks **Start**. `StartGame()` then enables gravity and hides the button. |
| (d) | **Jump** | **Space** applies an upward impulse, but only when the ball is on the ground (checked with a downward raycast). |
| (e) | **Power-up** | A `PowerUp` capsule appears once all pickups are collected. Collecting it doubles the player's speed, turns the ball red, and shows **"POWER-UP ACTIVE!"**. |
| (f) | **Breakable wall** | `BreakWall` (tag `BreakableWall`) blocks the path. While powered up, colliding with it destroys the wall. |
| (g) | **Goal / win** | Reaching the `Goal` trigger shows **"You win!"** and hides the player. |

### Requirements
- **Unity 6000.6.5f1** (Unity 6), installed through Unity Hub
- Packages, installed automatically from `Packages/manifest.json`: Universal Render Pipeline, Input System, TextMesh Pro / uGUI

### How to run
1. Clone the repository:
   ```bash
   git clone https://github.com/JHU-AR-2026/jcheng77_ar_2026_assignment4.git
   ```
2. In **Unity Hub**, click **Add → Add project from disk** and select the `Q2` folder.
3. Open the project with Unity **6000.6.5f1**. The first import regenerates the `Library/` folder, which can take a few minutes.
4. Open `Assets/Scenes/SampleScene.unity`.
5. Press **Play** in the editor.

### Controls
| Input | Action |
|-------|--------|
| Click **Start** | Begin the game (enables gravity and movement) |
| **W A S D** / arrow keys | Roll the ball |
| **Space** | Jump (only while grounded) |

### How to play
1. Click **Start**.
2. Collect all **10 gold cubes**. The counter at the top tracks your progress.
3. Once every cube is collected, a **power-up capsule** appears. Pick it up to gain speed and turn red.
4. While powered up, ram the **breakable wall** to destroy it.
5. Roll into the **goal** to win.****
