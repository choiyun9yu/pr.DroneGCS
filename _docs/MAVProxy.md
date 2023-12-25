# MAVProxy

MAVProxy는 Drone과 GCS 간의 통신을 중계하는 역할을 한다.

## 1. Start MAVProxy

### 1-1. Installation

    % sudo apt-get install python3-dev python3-opencv python3-wxgtk4.0 python3-pip python3-matplotlib python3-lxml python3-pygame
    % pip3 install PyYAML mavproxy --user
    % echo 'export PATH="$PATH:$HOME/.local/bin"' >> ~/.zshrc

    % mavproxy.py --master tcp:127.0.0.1:5762 --out udp:127.0.0.1:14556