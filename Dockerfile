FROM ubuntu:16.04

ENV DOTNET_CLI_TELEMETRY_OPTOUT=true \
    # Disable first time experience
    DOTNET_SKIP_FIRST_TIME_EXPERIENCE=true \
    # Configure Kestrel web server to bind to port 80 when present
    ASPNETCORE_URLS=http://+:80 \
    # Enable detection of running in a container
    DOTNET_RUNNING_IN_CONTAINER=true \
    # Enable correct mode for dotnet watch (only mode supported in a container)
    DOTNET_USE_POLLING_FILE_WATCHER=true \
    # Skip extraction of XML docs - generally not useful within an image/container - helps perfomance
    NUGET_XMLDOC_MODE=skip \
    GIT_SSH_VARIANT=ssh \
    USER=root \
    USERNAME=root \
    SUDO_GID=0 \
    SUDO_UID=0 \
    DotNetCore=/usr/bin/dotnet \
    DotNetCore_Path=/usr/bin/dotnet \
    SHELL=/bin/bash \
    COMPlus_EnableDiagnostics=0 \
    JAVA_HOME=/usr/lib/jvm/java-9-openjdk-amd64

RUN apt-get update && \
    apt-get install -y software-properties-common && \
    add-apt-repository -y ppa:openjdk-r/ppa && add-apt-repository -y ppa:git-core/ppa && apt-get update && \
    apt-get install -y git mercurial apt-transport-https ca-certificates telnet bzip2 vim

#install dotnet
RUN  apt-get update \
     && apt-get install -y wget \
     && wget -q https://packages.microsoft.com/config/ubuntu/16.04/packages-microsoft-prod.deb \
     && dpkg -i packages-microsoft-prod.deb \
     && apt-get install -y apt-transport-https \
     && apt-get update \
     && apt-get install -y dotnet-sdk-2.2

         #install dotnet 2.1
RUN  apt-get update \
     && apt-get install -y wget \
     && wget -q https://packages.microsoft.com/config/ubuntu/16.04/packages-microsoft-prod.deb \
     && dpkg -i packages-microsoft-prod.deb \
     && apt-get install -y apt-transport-https \
     && apt-get update \
     && apt-get install -y dotnet-sdk-2.1

#Install JAVA 9 Oracle
RUN wget https://download.java.net/java/GA/jdk9/9/binaries/openjdk-9_linux-x64_bin.tar.gz
RUN tar xzvf openjdk-9_linux-x64_bin.tar.gz
RUN mkdir -p /usr/lib/jvm
RUN mv jdk-9 /usr/lib/jvm/java-9-openjdk-amd64/
RUN update-alternatives --install /usr/bin/java java /usr/lib/jvm/java-9-openjdk-amd64/bin/java 1
RUN update-alternatives --install /usr/bin/javac javac /usr/lib/jvm/java-9-openjdk-amd64/bin/javac 1
RUN update-alternatives --display java
RUN update-alternatives --set  java /usr/lib/jvm/java-9-openjdk-amd64/bin/java
# Install killall & ftp
RUN apt-get install -y psmisc && \
    apt-get install -y ftp && \
    apt-get install -y sudo && \
    apt-get install -y sshpass && \
    apt-get install -y ncftp

EXPOSE 22
ADD startupe2e.sh /root/
RUN chmod 750 /root/startupe2e.sh
COPY . .

RUN dotnet restore
CMD chmod +x ./startupe2e.sh 
CMD ./startupe2e.sh