import System.Environment
import Data.List

-- | Flatten out the args into a string
flatten :: [[a]] -> [a]
flatten [] = []
flatten (x:xs) = x ++ flatten xs

main = do
  (file:rest) <- getArgs
  appendFile file $ flatten (intersperse " " rest)
  appendFile file "\n"
