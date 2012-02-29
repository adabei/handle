# -*- coding: utf-8 -*-
require 'albacore'

desc "List all available tasks"
task :default do
  puts 'All tasks:'
  puts Rake.application.tasks
end

desc "Tasks for WPF Client"
namespace :wpf do
  desc "Build WPF-Client"
  msbuild :build do |msb|
    msb.solution = "Handle.WPF/Handle.WPF.sln"
    msb.targets :clean, :build
    msb.properties :configuration => :debug
  end

  desc "Publish ClickOnce"
  msbuild :publish do |msb|
    msb.properties = {"configuration" => "Release",
                      "PublishDir" => "C:/Temp/",
                      "PublishUrl" => "C:/Temp/",
                      "InstallUrl" => "C:/Temp/"}
    msb.targets :Publish
    msb.solution = "./Handle.WPF.sln/"
  end

  desc "Run unit tests"
  nunit :unittests do |nunit|
    nunit.command = "Handle.WPF/packages/NUnit.2.5.10.11092/tools/nunit-console-x86.exe"
    nunit.assemblies "Handle.WPF/Handle.WPF.Test/bin/Debug/Handle.WPF.Test.dll"
  end
end

