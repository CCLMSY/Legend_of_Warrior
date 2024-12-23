# Legend_of_Warrior

## 简介/Introduction

一款基于Unity引擎的2D横版像素动作游戏。
勇士通过击杀怪物、穿越关卡取得胜利。

参考教程及素材来源：[Unity中文课堂](https://learn.u3d.cn/tutorial/2DAdventure)

## 操作方式/Operation

### 键盘/Keyboard

- `A` `D` / 方向键：左右移动（奔跑）
- `左Shift`：强制行走
- `Space`：跳跃
- `S`：下蹲
- `J`：攻击
- `K`：滑铲
- `L`：加载存档
- `E`：交互

### 手柄/Gamepad

- `Left Stick`：左右移动（奔跑）
- `Left Trigger`：强制行走
- `Button South`：跳跃
- `Button West`：攻击
- `Left Shoulder`：滑铲
- `Button East`：交互

## 玩家参数/Player

- 生命值：100
- 体力值：15
  - 恢复速度：1.2/s
- 攻击
  - 三段式攻击
  - 附带击退
  - 伤害：10/5/15
- 跳跃高度：3.5格
- 滑铲
  - 体力消耗：5
  - 距离：4.5格
  - 期间无敌

## 敌人/Enemy

| 敌人 | 生命值 | 伤害 | 速度 | 特性 |
| --- | --- | --- | --- | --- |
| 野猪 | 50 | 10 | 100 | 冲撞 |
| 蜗牛 | 30 | 10 | 60 | 护壳 |
| 蜜蜂 | 30 | 10 | 80 | 追击 |

- 冲撞：玩家进入攻击范围，进入冲撞状态
  - 冲撞速度：260
  - 持续时间：5秒
- 护壳：玩家进入攻击范围，进入护壳状态
  - 护壳时间：3秒
  - 期间无敌
  - 有碰撞伤害
- 追击：玩家进入攻击范围，进入追击状态
  - 追击速度：130
  - 持续时间：3秒
  - 无视地形
  - 追到玩家时，进行主动攻击，伤害：10

## 场景道具/Scene Props

### 瀑布/Waterfall

- 特性：玩家掉入瀑布，直接死亡

### 藤蔓/Vine

- 特性：玩家触碰藤蔓，受到伤害，伤害：10

### 传送门/Portal

- 特性：玩家进入传送门，传送至下一关卡

### 宝箱/Chest

- 特性：暂无

## 关卡/Level

当前关卡未经设计，仅用作核心逻辑展示

- 关卡1：森林
- 关卡2：洞穴

## 开发环境/Development Environment

- Unity 2022.3.43f1c1
- VSCode 1.94.2

## 安装/Installation

### 玩家

[>点击这里<](https://github.com/CCLMSY/Legend_of_Warrior/releases/download/v1.0/Legend_of_Warrior_v1.0.zip)下载并解压压缩包，双击`Legend_of_Warrior.exe`即可运行游戏。

### 开发者

1. 安装对应版本的Unity及VSCode，完成连接配置
2. 克隆仓库

  ```bash
  git clone https://github.com/CCLMSY/Legend_of_Warrior.git
  ```

1. 打开Unity，导入项目，可参考已有的脚本及资源进行地图设计、二次开发

## 演示/Demo

### 演示视频（Bilibili）

[《勇士传说》Demo](https://www.bilibili.com/video/BV19zyNYoEQR)

### 游戏截图

![Demo1](/DemoImg/Demo1.png)
![Demo2](/DemoImg/Demo2.png)
![Demo3](/DemoImg/Demo3.png)
![Demo4](/DemoImg/Demo4.png)
