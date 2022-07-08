# ML-Agents-Fighting-Animations

## Overview

Unity project with the results of the training of an artificial intelligence (AI) with the objective of creating simple humanoid procedural animations of defensive combat movements with a sword against an enemy attack using inverse kinematics (IK) and ML-Agents.

![Demo](https://user-images.githubusercontent.com/62213937/178018528-09814a96-a918-4655-a058-cb41da86b4ff.PNG)

## Software Versions

* Unity 2020.1.13f1
* ML-Agents Release 12

## Previous Knowledge
Useful links to learn some aspects of this project:
* [Inverse kinematics](https://docs.unity3d.com/2022.2/Documentation/Manual/InverseKinematics.html)
* [Machine learning and reinforcement learning](https://spinningup.openai.com/en/latest/spinningup/rl_intro.html)
* [Deep learning and deep reinforcement learning](https://arxiv.org/pdf/1901.05639.pdf)
* [ML-Agents](https://github.com/Unity-Technologies/ml-agents/tree/main/docs)

## Demo Explanation

The demo consists of a list of attack animations that can be selected from the drop-down menu that appears on the screen. When one is selected, it is reproduced and the samurai AI performs the procedural defense animation.

https://user-images.githubusercontent.com/62213937/178051228-0c21dc84-9eff-4415-a53e-a23f6d09b6e7.mp4

## Execute demo

Download the following ZIP ([build](https://drive.google.com/file/d/1dx8xU1aK_mhuS24vRmNdTdZoNsnAN1go/view?usp=sharing)) and execute "TFGProject.exe".

## Project Main Components 

* **Attacker:** A humanoid (a knight with a shield and sword) that has premade attack animations that hit the defender.

* **Defender (AI):** A humanoid (a samurai with his katana) that defense himself (moving the katana and blocking the enemy blow) using the AI.

## Creating the Animations

The animations are created by two components:
* An AI that applies a force to the katana in order to move it and make it collide with the attacker's sword at both midpoints of its blades. 
* IK applied to the defender's arms so that they follow the movement of the sword created by the AI.

## The AI

This artificial intelligence is trained by deep reinforcement learning, an artificial neural network (ANN) with a reinforcement learning algorithm, both implemented by the ML-Agents project.

### AI Configuration
The agent in this training environment can move and rotate the sword as if the samurai was moving it with his arms. In more technical terms, the actions that the agent can perform are 5 discrete actions ("move" in the X, Y, and Z axes, and "rotate" in the X and Z axes) with three options each (apply negative value, apply positive value, or not apply any new value).

As for his observations, from the environment, the agent will know the following.

**3D Position and rotation:** 
* Both swords
* Full body of the samurai divided in Head, Spine, Left and Right Arm, Hip, and Left and Right Leg

**Velocity:**
* Both swords

The next image shows how it is represented in Unity.

![InspectorAgent](https://user-images.githubusercontent.com/62213937/178067190-547cba3e-184e-4e39-9deb-3ae37c6f614e.PNG)

### Training Configuration

All the configuration data (reinforce algorithm used, ANN settings and hyperparameters, etc) is listed in the next image.

```YAML
behaviors:
  DefendSamurai:
    trainer_type: ppo
    hyperparameters:
      batch_size: 512
      buffer_size: 12000
      learning_rate: 0.0003
      beta: 0.005
      epsilon: 0.2
      lambd: 0.95
      num_epoch: 3
      learning_rate_schedule: linear
    network_settings:
      normalize: true
      hidden_units: 512
      num_layers: 3
      vis_encode_type: simple
      memory: null
    reward_signals:
      extrinsic:
        gamma: 0.99
        strength: 1.0
    init_path: null
    keep_checkpoints: 5
    checkpoint_interval: 500000
    max_steps: 30000000
    time_horizon: 1000
    summary_freq: 50000
```

See [Training Configuration File](https://github.com/Unity-Technologies/ml-agents/blob/release_19_docs/docs/Training-Configuration-File.md) in order to understand the previous image.

## Explore Project

### Software Requirements

* Unity 2020.1 or later
* Python 3.6.1 or later
* ML-Agents release 12 or later *

*See [ML-Agents: Installation](https://github.com/Unity-Technologies/ml-agents/blob/release_19_docs/docs/Installation.md)

### Installing
* Download the project and add it to Unity Hub.

## Acknowledgments
Third party resources:
* [Samurai and katana model](https://sketchfab.com/3d-models/samurai-0ac619b7b276445cb69c1887dd21ede9)
* [Knight model and attack animations](https://www.mixamo.com)
* [Tatami structure](https://sketchfab.com/3d-models/flying-island-low-poly-8780067653804460a0c792c91ed2bbf5)
* [Japanes leaf](https://www.textures.com/download/image/29264)
* [Skybox](https://assetstore.unity.com/packages/2d/textures-materials/sky/skybox-series-free-103633)
