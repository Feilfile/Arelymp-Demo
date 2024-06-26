chmod +x /root/linux_server/ArelympServer.x86_64
xvfb-run --auto-servernum --server-args='-screen 0 640X480X24:3
2' /root/linux_server/ArelympServer.x86_64 -batchmode -nographics