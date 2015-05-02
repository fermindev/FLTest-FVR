# FLTest-FVR
Fermin's work as candidate for Flexiand Limited

- Directories:

	* 'DNSResolver' contains C# sources (VS2010 solution).
	* 'Recipe' contains the single recipe (use: chef-apply dnsr.rb).
        * 'CookBook' directory contain the cookbook for installing and executing the program
           default.rb is overwriten with dnsr.rb content. 

Use: 

   chef-client --local-mode --runlist 'recipe[dnsr]'


Comments

- C# solution strongly based on article found in CodeProject: 
  http://www.codeproject.com/Articles/23673/DNS-NET-Resolver-C
  By Alphons van der Heijden.

- Recipe download file from GitHub to retrieve last version of the program.