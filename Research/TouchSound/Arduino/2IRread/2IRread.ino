void setup() {
  Serial.begin(9600);

}

void loop() {
  Serial.println(String(IRread(0) * CheckState(1),0));
  delay(1);
}

double IRread(int PinNo) {
 double ans ;
 int i ;
 ans = 0 ;
 for (i=0 ; i < 1000 ; i++) {
   ans  = ans + analogRead(PinNo) ;   // 指定のアナログピン(0番)から読取る
  }
  return 19501.14 * pow(ans/1000 , -1.256676) * 10;  //センサーの値を1000回読み取った平均値を返す(平滑化)
}

int CheckState(int PinNo) {
  int i;
  int ans = 0;
  int State;
  for (i=0; i<10 ; i++) {
    ans = ans + analogRead(PinNo);
  }
  if(ans>1200) State=1;
  else State = 0;
  return State;
}