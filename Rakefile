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
                       PublisherName: "HTLWRN",
                       ProductName: "Handle.WPF",
                       Install: "True",
                       ApplicationVersion: "1.0.0.*",
                       ApplicationRevision: "8",
                       GenerateManifests: "True",
                       TargetZone: "Internet",
                       IsWebBootstrapper: "True",
                       BootstrapperEnabled: "True",
                       SupportUrl: "http://www2.htlwrn.ac.at/handle/",
                       UpdateEnabled: "False" }
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

