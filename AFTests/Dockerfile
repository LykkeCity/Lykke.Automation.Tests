FROM microsoft/aspnetcore-build:2.0

EXPOSE 22
ADD startup.sh /root/
RUN chmod 750 /root/startup.sh
COPY . .
CMD ["bash","/root/startup.sh"]