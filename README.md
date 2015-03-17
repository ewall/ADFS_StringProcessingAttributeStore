ADFS String Processing Attribute Store
======================================

    This ADFS Custom Attribute Store would allow us to transform claim rules with basic string manipulation, such as changing to all lowercase or all caps.
    
The code was initially straight from Microsoft's example here:

* [https://msdn.microsoft.com/en-us/library/hh599320.aspx]()

...with assistance from this article:

* [http://blogs.technet.com/b/cloudpfe/archive/2013/12/27/how-to-create-a-custom-attribute-store-for-active-directory-federation-services-3-0.aspx]()

See those pages for details on compiling, installing, and using the DLL.

## Change History ##
 
 - 1.0.1 Added a few new methods: trim, removeDashes, truncateTo12Chars, truncateTo16Chars
 - 1.0.0 Code matches Microsoft's string processing example exactly.
 
## To Do ##

 - Re-implement `truncate` as a 2-paramter query, reference: [https://msdn.microsoft.com/en-us/library/ee895358.aspx]()

