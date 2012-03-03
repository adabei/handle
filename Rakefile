# encoding: UTF-8
require 'albacore'

desc "List all available tasks"
task :default do
  puts 'All tasks:'
  puts Rake.application.tasks
end

desc "Tasks for the WPF client"
namespace :wpf do

  desc "Build the client with the debug configuration"
  msbuild :build do |msb|
    msb.solution = "Handle.WPF/Handle.WPF.sln"
    msb.targets :Clean, :Build
    msb.properties Configuration: "Debug"
  end

  desc "Publish ClickOnce"
  msbuild :publish do |msb|
    msb.properties = { Configuration: "Release",
                       PublishUrl: "http://www2.htlwrn.ac.at/handle/setup/",
                       TargetZone: "Internet",
                       BootstrapperEnabled: "True",
                       IsWebBootstrapper: "True",
                       SupportUrl: "http://www2.htlwrn.ac.at/handle/",
                       InstallUrl: "http://www2.htlwrn.ac.at/handle/setup/",
                       UpdateEnabled: "True",
                       UpdateUrl: "http://www2.htlwrn.ac.at/handle/setup/" }
    msb.targets :Publish
    msb.solution = "Handle.WPF/Handle.WPF.sln"
  end

  desc "Run unit tests"
  nunit :unittests do |nunit|
    nunit.command = "Handle.WPF/packages/NUnit.2.5.10.11092/tools/nunit-console-x86.exe"
    nunit.assemblies "Handle.WPF/Handle.WPF.Test/bin/Debug/Handle.WPF.Test.dll"
  end

  desc "Generate documentation using Doxygen"
  task :doc do
    system 'doxygen Handle.WPF/Doxyfile'
  end

end

