#include <Wire.h>
#include <DHT.h>
#include <LiquidCrystal_I2C.h>

LiquidCrystal_I2C lcd(0x27, 16, 2);  //Lưu ý: khai báo địa chỉ LCD tương ứng
#include "DHT.h"      //gọi thư viện DHT11
const int DHTPIN = 2;       //Đọc dữ liệu từ DHT11 ở chân 2 trên mạch Arduino
const int DHTTYPE = DHT11;  //Khai báo loại cảm biến, có 2 loại là DHT11 và DHT22
DHT dht(DHTPIN, DHTTYPE);

int quangtro = A1; 
int coil = 8;
int heater = 9;
int led = 10;

int in3 = 5; 
int in4 = 4; 
int EnB = 3; 


int sp1 = 50;
int sp2 = 150;
int sp3 = 255;

///////////////////////////////////////
void setup() {
   pinMode(EnB,OUTPUT);
   pinMode(in3, OUTPUT);
   pinMode(in4, OUTPUT);
   pinMode(coil, OUTPUT);
   pinMode(heater, OUTPUT);
   pinMode(led, OUTPUT);
   digitalWrite(coil, HIGH);
   digitalWrite(heater, HIGH);
   digitalWrite(led, HIGH);
   

  Serial.begin(9600);
  dht.begin();         // Khởi động cảm biến
  lcd.init();
  lcd.backlight();

}
////////////////////////////////////////
void loop() {
  digitalWrite(in4, LOW);
  digitalWrite(in3, HIGH);
  float humidity = dht.readHumidity();    
  float temperature = dht.readTemperature();
  int ldr = analogRead(quangtro);// đọc giá trị quang trở    

  if (isnan(humidity) || isnan(temperature)) { // Kiểm tra xem thử việc đọc giá trị có bị thất bại hay không.
  } 
  else {
  Serial.print(temperature, 1); 
  Serial.print(",");
  Serial.print(humidity, 1);
  Serial.print(",");
  Serial.print(ldr, 1); 
  Serial.println(); 
  lcd.setCursor(0, 0);
  lcd.print("Temp:");
  lcd.setCursor(7, 0);
  lcd.print(temperature);
  lcd.setCursor(12, 0);
  lcd.write(0xDF);
  lcd.setCursor(13, 0);
  lcd.print("C");
  lcd.setCursor(0, 1);
  lcd.print("Humi:");
  lcd.setCursor(7, 1);
  lcd.print(humidity);
  lcd.setCursor(12, 1);
  lcd.print("%");
  delay(200);
  }
  lcd.setCursor(5, 0);
    delay(200);
    ////////////////////////////
    if (Serial.available() > 0){
    int value = Serial.read();

    switch (value){
      case '0':
          analogWrite(EnB, 0);
          break;
      case '1':
          analogWrite(EnB, sp1);
          break;
      case '2':
          analogWrite(EnB, sp2);
          break;
      case '3':
          analogWrite(EnB, sp3);
          break;
      case 'C':
          digitalWrite(coil, LOW); //bật valve
          break;
      case 'D':
          digitalWrite(coil, HIGH);
          break;
      case 'H':
          digitalWrite(heater, LOW); //bật heater
          break;
      case 'I':
          digitalWrite(heater, HIGH);
          break;
      case 'L':
          digitalWrite(led, LOW);  //bật led
          break;
      case 'M':
          digitalWrite(led, HIGH);
          break;
    }
  }
}
