echo "Fuck JavaScript"
echo "tsc is running!"
$fs = ls;
foreach ($f in $fs) {
  if($f.Extension.ToLower().Equals(".ts")){
    tsc --outFile "../js/$($f.Name.SubString(0,$f.Name.Length-$f.Extension.Length)).js" $f.Name
  }
}
echo "tsc is done!"