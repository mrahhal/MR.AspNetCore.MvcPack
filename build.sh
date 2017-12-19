#header "Adjusting file descriptors limit, if necessary"
#FILE_DESCRIPTOR_LIMIT=$( ulimit -n )
#if [ $FILE_DESCRIPTOR_LIMIT -lt 512 ]
#then
#	info "Increasing file description limit to 512"
#	ulimit -n 512
#fi

dotnet --info
dotnet restore
dotnet test test/MR.AspNetCore.MvcPack.Tests/MR.AspNetCore.MvcPack.Tests.csproj -f netcoreapp2.0
