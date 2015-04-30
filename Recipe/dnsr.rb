directory 'C:\\DNSR' do
	action :create
end

remote_file 'C:\DNSR\DNSResolver.exe' do
	source 'https://github.com/fermindev/FLTest-FVR/blob/master/DNSResolver/DNSResolver/bin/Release/DNSResolver.exe?raw=true'
end

remote_file 'C:\DNSR\query.txt' do
	source 'https://github.com/fermindev/FLTest-FVR/raw/master/DNSResolver/DNSResolver/bin/Release/query.txt'
end

batch 'Run DNSResolver using query.txt as input' do
	code 'C:\\DNSR\\DNSResolver.exe C:\\DNSR\\query.txt C:\\DNSR\\resolved.txt'
	action :run
end