[33mcommit 5c090ace81a68afa3e29f80e392ac1da198ce0b9[m[33m ([m[1;36mHEAD -> [m[1;32mmain[m[33m)[m
Merge: bdffb96 2c021f8
Author: Gordon Arber <Gordon.arber@gmail.com>
Date:   Fri Oct 23 16:14:43 2020 -0400

    Merge branch 'main' of https://github.com/GordonArber/open-project-1 into main

[33mcommit bdffb96a69389fa4a26c2ce57420c54990d0c1fa[m
Author: Gordon Arber <Gordon.arber@gmail.com>
Date:   Fri Oct 23 15:50:18 2020 -0400

    updated the names of all the private fields to match documentation

[33mcommit 2c021f829693d5cb907e59ae6fab89958dae1849[m
Author: GordonArber <Gordon.arber [at]  gmail.com>
Date:   Fri Oct 23 15:50:18 2020 -0400

    updated the names of all the private fields to match documentation

[33mcommit 6f0cb6c2554de6b4fc6976195ada6907aa97805d[m
Author: Gordon Arber <nodrogames@gmail.com>
Date:   Thu Oct 22 17:39:43 2020 -0400

    Changed all the public fields to [SerializeField] private
    
    According to the Conventions document - We are to use the attribute [SerializeField] and private, instead of making them public.

[33mcommit 3f166435c9f81394234c603c22eda927a2eae664[m[33m ([m[1;31mupstream/main[m[33m)[m
Author: Dave Rodriguez <drod7425@gmail.com>
Date:   Tue Oct 20 15:14:44 2020 -0400

    Added Conventions Link to README (#97)
    
    * Added Conventions document link to README
    
    * Small tweaks to the readme wording

[33mcommit 57f3790dc5f8bbe7aee9f8e009e41c56b5785467[m
Author: Dave Rodriguez <drod7425@gmail.com>
Date:   Wed Oct 14 09:51:33 2020 -0400

    Update README.md (#82)
    
    Added a permanent and non-capped invite link to the #open-projects Discord channel on the Official Unity Discord server.

[33mcommit c75cb382e87f26255625123b8abf3690cf7918ea[m
Author: Ciro Continisio <ciro@unity3d.com>
Date:   Wed Oct 14 14:38:12 2020 +0200

    Project settings
    
    Small tweaks, project naming, etc.

[33mcommit dd25e2f98c715ffdad41c4b88fcd9fe458fb1e0a[m
Author: Wikum Chamith <wikum@linuxdeveloper.space>
Date:   Mon Oct 12 00:34:45 2020 +0530

    Removed unused imports (#75)

[33mcommit 22307243b0ad00ca2d2e72717f8028a24b204c9e[m
Author: Megatank58 <51410502+MEGATANK58@users.noreply.github.com>
Date:   Thu Oct 8 14:47:04 2020 +0530

    Removal of unnecessary test files (#80)
    
    Removal of unnecessary Tests folder, Input Scene and the content in the Scene. I have tested the game and it doesn't breaks or effects the game in any possible way.

[33mcommit 6cadca6fb22c8d97f68c8aced6197d550aea9234[m
Author: Miley Hollenberg <miley.hollenberg@unity3d.com>
Date:   Wed Oct 7 22:57:14 2020 +0300

    Moved .editorConfig to avoid duplicated version in actual project root (#77)

[33mcommit a056f34caa2aa22231139da235bee47446e9abec[m
Merge: 067e8dc 8a32292
Author: Ciro Continisio <ciro@unity3d.com>
Date:   Wed Oct 7 10:29:24 2020 +0200

    Merge pull request #73 from MileyHollenberg/Linter
    
    Setup automatic linting as a Github Action

[33mcommit 067e8dc0a94f5bd97b19b4dde73f9cdeb5bfcf30[m
Author: Ciro Continisio <ciro@unity3d.com>
Date:   Tue Oct 6 20:26:53 2020 +0200

    Create pull_request_template.md

[33mcommit 8a322921074df84146bea813bd064c25f8327e92[m
Author: Miley Hollenberg <miley.hollenberg@unity3d.com>
Date:   Tue Oct 6 14:59:58 2020 +0000

    [Bot] Automated dotnet-format update

[33mcommit 1c79e701c7d936cf0c41977b30eb5b06a60a0ed2[m
Author: Miley Hollenberg <miley.hollenberg@unity3d.com>
Date:   Tue Oct 6 17:59:13 2020 +0300

    Removed bot account from auto commit

[33mcommit ea184cd00c62cd78e3db58d8f6bd02d1a38d0228[m
Author: Miley Hollenberg <mileyhollenberg@gmail.com>
Date:   Sun Oct 4 19:04:42 2020 +0300

    Implemented automatic linter

[33mcommit ba8db819b48a922f17e77a4224e0f415e70c1acb[m
Merge: 219f2d9 c589868
Author: Ciro Continisio <ciro@unity3d.com>
Date:   Tue Oct 6 01:06:43 2020 +0200

    Merge pull request #26 from mwert09/master
    
    Fixed steep surfaces bug and implemented sliding

[33mcommit c589868a7a6f828550e6312a55d62d708ee21894[m
Merge: 967d197 219f2d9
Author: mwert09 <mwert09@gmail.com>
Date:   Mon Oct 5 12:45:48 2020 +0300

    Merge remote-tracking branch 'upstream/master'

[33mcommit 219f2d955c191753f03d6657e867a2fa27dfd8fb[m
Merge: 6cabf93 b71a120
Author: Ciro Continisio <ciro@unity3d.com>
Date:   Sun Oct 4 23:41:21 2020 +0200

    Merge pull request #55 from andrewreal/remove-magic-numbers
    
    Remove magic numbers

[33mcommit b71a1208948a8e5bd18d6e9b102575f7eba30fba[m
Author: Ciro Continisio <ciro@unity3d.com>
Date:   Sun Oct 4 23:40:22 2020 +0200

    Changed the new variable into a constant

[33mcommit 6cabf933aaf06686458c18bd7d090526eb2596f5[m
Merge: 753b2ea 97d64b1
Author: Ciro Continisio <ciro@unity3d.com>
Date:   Sun Oct 4 23:05:36 2020 +0200

    Merge pull request #25 from Wodopo/master
    
    Simple Player Spawn System

[33mcommit 97d64b14be85b164f5c474b5e47e4ea2e26a8e75[m
Author: Ciro Continisio <ciro@unity3d.com>
Date:   Sun Oct 4 23:00:26 2020 +0200

    SpawnSystem active in the scene
    
    Removed the player prefab from the scene

[33mcommit 8e5208404bac792b373f3c1c9c443962c2ec0286[m
Merge: 5ab6971 753b2ea
Author: Ciro Continisio <ciro@unity3d.com>
Date:   Sun Oct 4 22:50:52 2020 +0200

    Merge branch 'master' into Wodopo/spawn-system
    
    # Conflicts:
    #       UOP1_Project/Assets/Scenes/CharController.unity
    #       UOP1_Project/Assets/Scripts/CameraManager.cs

[33mcommit 753b2ea27f0674f792afa7a8f80168e45dad7754[m
Merge: 21bd0e2 ab84ea0
Author: Ciro Continisio <ciro@unity3d.com>
Date:   Sun Oct 4 22:46:38 2020 +0200

    Merge pull request #27 from andrew-raphael-lukasik/InputReader_as_ScriptableObject
    
    InputReader made into ScriptableObject asset

[33mcommit 5ab69716ea841a6e4af1e9a11f3fc50ee3ba95aa[m
Author: Ciro Continisio <ciro@unity3d.com>
Date:   Sun Oct 4 22:41:40 2020 +0200

    Spawn system in place

[33mcommit 660f2b270d72edf0580b238de157dfc6b70c60c4[m
Author: Andrew Real <andrew@andrewreal.com>
Date:   Sun Oct 4 10:20:53 2020 +0100

    Remove accidentaly added blank lines

[33mcommit e2aecd9cf4d984c2231475c5e04ba6b7eb2056d2[m
Author: Andrew Real <andrew@andrewreal.com>
Date:   Sun Oct 4 10:18:11 2020 +0100

    Change var name to more acurately show purpose

[33mcommit 109d937d9a55539acc8851b1ee035bfeeb8c2e89[m
Merge: 945aaf8 21bd0e2
Author: Andrew Real <andrew@andrewreal.com>
Date:   Sat Oct 3 15:04:54 2020 +0100

    Merge branch 'master' into remove-magic-numbers

[33mcommit 967d197a835059df870016d596bb347eeb6425b4[m
Merge: a99b6b0 21bd0e2
Author: mwert09 <mwert09@gmail.com>
Date:   Sat Oct 3 12:46:26 2020 +0300

    Merge remote-tracking branch 'upstream/master'

[33mcommit a99b6b0ab3d95a6888ab5475fe19b8480229db9d[m
Merge: 4d55d0f 397090f
Author: mwert09 <mwert09@gmail.com>
Date:   Sat Oct 3 12:34:05 2020 +0300

    Merge branch 'bug-steep-surfaces'

[33mcommit 397090feeb9267c5b08f5953aebefa2a85a8ab03[m
Author: mwert09 <mwert09@gmail.com>
Date:   Sat Oct 3 12:33:27 2020 +0300

    Fixed bouncing issue when player hit under floating platform

[33mcommit 21bd0e20187b0e13f39c7c01403562f4bf657286[m
Merge: 6afc323 9bf0250
Author: Ciro Continisio <ciro@unity3d.com>
Date:   Sat Oct 3 03:04:50 2020 +0200

    Merge pull request #37 from Acimaz/TMPro-Import
    
    Imported Textmesh Pro Essentials

[33mcommit 9bf02504a88e30b63986ee79fed94072be6cc62b[m
Merge: 4b27f4a 6afc323
Author: Ciro Continisio <ciro@unity3d.com>
Date:   Sat Oct 3 03:03:53 2020 +0200

    Merge branch 'master' into TMPro-Import

[33mcommit 6afc323fadb6ffa47e98de36023729c061eae2ad[m
Merge: 1fe3d15 309d7cd
Author: Ciro Continisio <ciro@unity3d.com>
Date:   Sat Oct 3 02:02:37 2020 +0200

    Merge pull request #45 from Quickz/jump-fix
    
    Fixed an issue regarding player jump not getting canceled in a few cases

[33mcommit 309d7cdf004f4a648209d9b9b7502eb64f373f58[m
Author: Quickz <alexserg@inbox.lv>
Date:   Sat Oct 3 02:22:20 2020 +0300

    fixed an issue regarding player jump not getting canceled in a few cases

[33mcommit 1fe3d15f6d7f5a5867f6d08067e8535a08e286f7[m
Author: Ciro Continisio <ciro@unity3d.com>
Date:   Sat Oct 3 00:19:23 2020 +0200
