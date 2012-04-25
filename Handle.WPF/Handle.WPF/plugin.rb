class Plugin
  @@version ="1.00"

  @@handle = { authors: ["Bernhard Schwarz", "Florian Lembeck"], 
             name: "plugin.rb",
             description: "Base class for Handle plugins" }

  def version
    @@version
  end

  def handle
    @@handle
  end

  def self_join
  end
  
  def pre_self_part
  end
  
  def join
  end

  def part
  end

  def nick
  end
  
  def self_nick
  end

  def pre_message_sent
  end

  def message_sent
  end

  def pre_message_received
  end

  def message_received
  end
end
