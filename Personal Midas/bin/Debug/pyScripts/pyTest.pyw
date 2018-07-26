import btctrade
publicKey = 'YOUR PUBLIC KEY'
f = open('data/privatekey.dat')
privateKey = f.read();
btc = btctrade.Btctrade(publicKey, privateKey)
print(btc.get_balance())
end = input()
