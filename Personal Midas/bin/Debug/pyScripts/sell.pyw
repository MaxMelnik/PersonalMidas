import btctrade
publicKey = 'YOUR PUBLIC KEY'
f = open('pyScripts/data/privatekey.dat')
privateKey = f.read();
f = open('pyScripts/tmp/price.dat')
price = f.read();
f = open('pyScripts/tmp/count.dat')
count = f.read();
btc = btctrade.Btctrade(publicKey, privateKey)
request = btc.sell("btc_uah", price, count)
f = open('pyScripts/tmp/sellOrderId.dat', 'w')
orderId = str(request['order_id'])
f.write(orderId)
