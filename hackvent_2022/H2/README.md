# H2 - Santa's Secret

## Description

Level: Medium<br/>

## Solution

The second hidden flag could be found through the AWS metadata API by requesting a screenshot of
`http://169.254.169.254/latest/meta-data/tags/instance/hidden_flag`.

