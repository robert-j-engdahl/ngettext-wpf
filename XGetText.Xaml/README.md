How to Run the Tests
====================

Make sure the nuget packages of the solution are restored and then invoke

    > pester
    
from the Package Manager Console.

If you try the more conventional `Invoke-Pester` you might get all kinds of errors, because your local Pester installation might not be compatible with the Pester tests in this repository.
