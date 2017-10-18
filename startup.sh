#!/bin/bash
searchString="Total tests:"
url="https://hooks.slack.com/services/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"
H="'Content-type: application/json'"

b=$(dotnet vstest AFTests.dll)
echo "test run finished"

echo "{\"attachments\": [{\"text\":\"" > tmp1.txt
b=$(echo $b | tr "'" "“")
c=$(echo $b | grep -o "$searchString.*")
echo $b >> tmp1.txt
echo "\"}],\"text\":\"" >> tmp1.txt
echo $c >> tmp1.txt
echo "\"}" >> tmp1.txt
cat tmp1.txt | sed 's/\\/\\\\/g' > tmp2.txt
cat tmp2.txt | sed 's/\r/\n/g' > tmp3.txt
cat tmp3.txt | sed 's/\n/\\n/g' > tmp4.txt

echo "Sending resutls to Slack"
echo "curl -X POST -H $H --data-binary @tmp4.txt $url"
curl -X POST -H $H --data-binary @tmp4.txt $url >out 2>&1

#tail tmp4.txt

echo "Sleeping..."
sleep infinity