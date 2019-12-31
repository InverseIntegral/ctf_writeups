# H1 - Hidden One

## Description

Author: hidden

Sometimes, there are hidden flags. Got your first?

## Solution

Challenge [06](../06) contained a section of text that could be copied.

![Text table](text.png)

I copied the text and looked at it in vim. I enabled `:set list` to see spaces and tabs and realized that there were
additional characters  at the end of each line:

```
Born: January 22	     	 	   	   	 	       	     	  	  
Died: April 9   	  	 	    	  	      	   		  	  
Mother: Lady Anne   		 	   	   	      	  	      	  
Father: Sir Nicholas	 	      		    	    	  	  	      	      
Secrets: unknown      	 	  	 	    	    	   	       	  
```

I assumed that this was whitespace steganography and therefore I used [SNOW](http://www.darkside.com.au/snow/) to get
the flag `HV19{1stHiddenFound}`.
