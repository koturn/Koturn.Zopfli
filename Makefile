SOLUTION_NAME = Koturn.Zopfli
SOLUTION_FILE = $(SOLUTION_NAME).sln
MAIN_PROJECT_FILE = $(SOLUTION_NAME)\$(SOLUTION_NAME).csproj
TEST_PROJECT_FILE = Koturn.Zopfli.Tests\Koturn.Zopfli.Tests.csproj
PROJECT_DIRS = $(SOLUTION_NAME)
CXX_PROJECT_DIRS = libzopfli libzopflipng
ARTIFACTS_BASEDIR = Artifacts
ARTIFACTS_SUBDIR_BASENAME = $(SOLUTION_NAME)
ARTIFACTS_BASENAME = $(SOLUTION_NAME)
BUILD_CONFIG = Release
BUILD_PLATFORM = "Any CPU"
TARGET_NETSTD20 = netstandard2.0
TARGET_NET8 = net8.0
TARGET_NET9 = net9.0
TARGET_NET10 = net10.0
RM = del /F /Q
RMDIR = rmdir /S /Q


all: build

build:
	dotnet build -c $(BUILD_CONFIG) $(MAIN_PROJECT_FILE)
	msbuild /nologo /m /p:Configuration=$(BUILD_CONFIG);Platform=x64 libzopfli\libzopfli.vcxproj
	msbuild /nologo /m /p:Configuration=$(BUILD_CONFIG);Platform=x86 libzopfli\libzopfli.vcxproj
	msbuild /nologo /m /p:Configuration=$(BUILD_CONFIG);Platform=x64 libzopflipng\libzopflipng.vcxproj
	msbuild /nologo /m /p:Configuration=$(BUILD_CONFIG);Platform=x86 libzopflipng\libzopflipng.vcxproj

restore:
	dotnet restore $(SOLUTION_FILE)

test:
	dotnet test -c $(BUILD_CONFIG) --logger:Console;verbosity=detailed $(TEST_PROJECT_FILE)

deploy: deploy-$(TARGET_NETSTD20) deploy-$(TARGET_NET8) deploy-$(TARGET_NET9) deploy-$(TARGET_NET10)

deploy-$(TARGET_NETSTD20):
	-dotnet publish -c $(BUILD_CONFIG) -f $(TARGET_NETSTD20) \
		-p:PublishDir=..\$(ARTIFACTS_BASEDIR)\$(ARTIFACTS_SUBDIR_BASENAME)-$(TARGET_NETSTD20) \
		$(MAIN_PROJECT_FILE)
	-$(RM) $(ARTIFACTS_BASEDIR)\$(ARTIFACTS_SUBDIR_BASENAME)-$(TARGET_NETSTD20)\*.pdb \
		$(ARTIFACTS_BASENAME)-$(TARGET_NETSTD20).zip 2>NUL
	xcopy /d /i /S /Y "$(SOLUTION_NAME)\bin\$(BUILD_CONFIG)\$(TARGET_NETSTD20)\x64\*.dll" "$(ARTIFACTS_BASEDIR)\$(ARTIFACTS_SUBDIR_BASENAME)-$(TARGET_NETSTD20)\x64\"
	xcopy /d /i /S /Y "$(SOLUTION_NAME)\bin\$(BUILD_CONFIG)\$(TARGET_NETSTD20)\x86\*.dll" "$(ARTIFACTS_BASEDIR)\$(ARTIFACTS_SUBDIR_BASENAME)-$(TARGET_NETSTD20)\x86\"
	cd $(ARTIFACTS_BASEDIR)
	powershell Compress-Archive -Path $(ARTIFACTS_SUBDIR_BASENAME)-$(TARGET_NETSTD20) -DestinationPath ..\$(ARTIFACTS_BASENAME)-$(TARGET_NETSTD20).zip
	cd $(MAKEDIR)

deploy-$(TARGET_NET8):
	-dotnet publish -c $(BUILD_CONFIG) -f $(TARGET_NET8) \
		-p:PublishDir=..\$(ARTIFACTS_BASEDIR)\$(ARTIFACTS_SUBDIR_BASENAME)-$(TARGET_NET8) \
		$(MAIN_PROJECT_FILE)
	-$(RM) $(ARTIFACTS_BASEDIR)\$(ARTIFACTS_SUBDIR_BASENAME)-$(TARGET_NET8)\*.pdb \
		$(ARTIFACTS_BASENAME)-$(TARGET_NET8).zip 2>NUL
	xcopy /d /i /S /Y "$(SOLUTION_NAME)\bin\$(BUILD_CONFIG)\$(TARGET_NET8)\x64\*.dll" "$(ARTIFACTS_BASEDIR)\$(ARTIFACTS_SUBDIR_BASENAME)-$(TARGET_NET8)\x64\"
	xcopy /d /i /S /Y "$(SOLUTION_NAME)\bin\$(BUILD_CONFIG)\$(TARGET_NET8)\x86\*.dll" "$(ARTIFACTS_BASEDIR)\$(ARTIFACTS_SUBDIR_BASENAME)-$(TARGET_NET8)\x86\"
	cd $(ARTIFACTS_BASEDIR)
	powershell Compress-Archive -Path $(ARTIFACTS_SUBDIR_BASENAME)-$(TARGET_NET8) -DestinationPath ..\$(ARTIFACTS_BASENAME)-$(TARGET_NET8).zip
	cd $(MAKEDIR)

deploy-$(TARGET_NET9):
	-dotnet publish -c $(BUILD_CONFIG) -f $(TARGET_NET9) \
		-p:PublishDir=..\$(ARTIFACTS_BASEDIR)\$(ARTIFACTS_SUBDIR_BASENAME)-$(TARGET_NET9) \
		$(MAIN_PROJECT_FILE)
	-$(RM) $(ARTIFACTS_BASEDIR)\$(ARTIFACTS_SUBDIR_BASENAME)-$(TARGET_NET9)\*.pdb \
		$(ARTIFACTS_BASENAME)-$(TARGET_NET9).zip 2>NUL
	xcopy /d /i /S /Y "$(SOLUTION_NAME)\bin\$(BUILD_CONFIG)\$(TARGET_NET9)\x64\*.dll" "$(ARTIFACTS_BASEDIR)\$(ARTIFACTS_SUBDIR_BASENAME)-$(TARGET_NET9)\x64\"
	xcopy /d /i /S /Y "$(SOLUTION_NAME)\bin\$(BUILD_CONFIG)\$(TARGET_NET9)\x86\*.dll" "$(ARTIFACTS_BASEDIR)\$(ARTIFACTS_SUBDIR_BASENAME)-$(TARGET_NET9)\x86\"
	cd $(ARTIFACTS_BASEDIR)
	powershell Compress-Archive -Path $(ARTIFACTS_SUBDIR_BASENAME)-$(TARGET_NET9) -DestinationPath ..\$(ARTIFACTS_BASENAME)-$(TARGET_NET9).zip
	cd $(MAKEDIR)

deploy-$(TARGET_NET10):
	-dotnet publish -c $(BUILD_CONFIG) -f $(TARGET_NET10) \
		-p:PublishDir=..\$(ARTIFACTS_BASEDIR)\$(ARTIFACTS_SUBDIR_BASENAME)-$(TARGET_NET10) \
		$(MAIN_PROJECT_FILE)
	-$(RM) $(ARTIFACTS_BASEDIR)\$(ARTIFACTS_SUBDIR_BASENAME)-$(TARGET_NET10)\*.pdb \
		$(ARTIFACTS_BASENAME)-$(TARGET_NET10).zip 2>NUL
	xcopy /d /i /S /Y "$(SOLUTION_NAME)\bin\$(BUILD_CONFIG)\$(TARGET_NET10)\x64\*.dll" "$(ARTIFACTS_BASEDIR)\$(ARTIFACTS_SUBDIR_BASENAME)-$(TARGET_NET10)\x64\"
	xcopy /d /i /S /Y "$(SOLUTION_NAME)\bin\$(BUILD_CONFIG)\$(TARGET_NET10)\x86\*.dll" "$(ARTIFACTS_BASEDIR)\$(ARTIFACTS_SUBDIR_BASENAME)-$(TARGET_NET10)\x86\"
	cd $(ARTIFACTS_BASEDIR)
	powershell Compress-Archive -Path $(ARTIFACTS_SUBDIR_BASENAME)-$(TARGET_NET10) -DestinationPath ..\$(ARTIFACTS_BASENAME)-$(TARGET_NET10).zip
	cd $(MAKEDIR)

clean:
	-for %%d in ( $(PROJECT_DIRS) ) do @( \
		@$(RMDIR) %%d\bin %%d\obj 2>NUL \
	)
	-for %%d in ( $(CXX_PROJECT_DIRS) ) do @( \
		@$(RMDIR) %%d\x86 %%d\x64 2>NUL \
	)

distclean: clean
	-$(RMDIR) $(ARTIFACTS_BASEDIR) 2>NUL
	-$(RM) $(ARTIFACTS_BASENAME)-*.zip 2>NUL
