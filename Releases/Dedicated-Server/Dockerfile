FROM ubuntu:22.04
MAINTAINER MatthiasFeil

ARG debian_frontend=noninteractive
ARG docker_version=17.06.0-ce

RUN apt-get update && \
    apt-get install -y libglu1 xvfb libxcursor1 ca-certificates && \
    apt-get clean && \
    update-ca-certificates

COPY linux_server/ /root/linux_server/
COPY boot.sh /boot.sh

WORKDIR /root/
ENTRYPOINT ["/bin/bash", "/boot.sh"]