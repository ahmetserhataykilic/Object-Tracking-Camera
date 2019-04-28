#include <16F877A.h>

#device ADC=10
#fuses XT,NOWDT,NOPROTECT,NOBROWNOUT,NOLVP,NOPUT,NOWRT,NODEBUG,NOCPD
#use delay(crystal=20000000)
#use rs232(baud=9600,xmit=PIN_C6,rcv=PIN_C7,bits=8,parity=N, stop=1)
                   //No low voltage prgming, B3(PIC16) or B5(PIC18) used for I/O
#include <stdlib.h>



// A�IKLAMALAR
// Xdir pin=High ise sa�a d�ner, Zdir pin H�gh ise yukar� d�ner. enable pin HIGH ise motor pasif olur.

// De�i�ken Tan�mlamalar�
#define stepX_dir pin_c4
#define stepX_step pin_d3
#define stepX_E pin_d2
#define stepZ_dir pin_c3
#define stepZ_step pin_d0
#define stepZ_E pin_d1
#define led1x1 pin_b5
#define led1x2 pin_b4
#define led1x3 pin_b3
#define led2x1 pin_b2
#define led2x2 pin_b1
#define led2x3 pin_b0
#define led3x1 pin_d7
#define led3x2 pin_d6
#define led3x3 pin_d5

char gelenVeri;
// OTOMAT�K MOD DE���KENLER
int16 adim=30, pulseSure=1000;
// MANUEL MOD DE���KENLER
int16 adim1=100, pulseSure1=1000, pulseSure3=400;
// EVE G�T DE���KENLER
signed int16 xSayac=0, zSayac=0;
int16 adimSag, adimSol, adimUst, adimAlt;
//////-----------//////
int16 i,j,k,b,a,c,d,e,f,g,h,n;
int sayma=1;





void sagust()
{
     output_low(led1x1);
     output_low(led1x2); 
     output_high(led1x3); 
     output_low(led2x1); 
     output_low(led2x2); 
     output_low(led2x3); 
     output_low(led3x1); 
     output_low(led3x2); 
     output_low(led3x3); 
}
void ust()
{
     output_low(led1x1);
     output_high(led1x2); 
     output_low(led1x3); 
     output_low(led2x1); 
     output_low(led2x2); 
     output_low(led2x3); 
     output_low(led3x1); 
     output_low(led3x2); 
     output_low(led3x3); 
}
void solust()
{
     output_high(led1x1);
     output_low(led1x2); 
     output_low(led1x3); 
     output_low(led2x1); 
     output_low(led2x2); 
     output_low(led2x3); 
     output_low(led3x1); 
     output_low(led3x2); 
     output_low(led3x3); 
}
void sag()
{
     output_low(led1x1);
     output_low(led1x2); 
     output_low(led1x3); 
     output_low(led2x1); 
     output_low(led2x2); 
     output_high(led2x3); 
     output_low(led3x1); 
     output_low(led3x2); 
     output_low(led3x3); 
}
void orta()
{
     output_low(led1x1);
     output_low(led1x2); 
     output_low(led1x3); 
     output_low(led2x1); 
     output_high(led2x2); 
     output_low(led2x3); 
     output_low(led3x1); 
     output_low(led3x2); 
     output_low(led3x3); 
}
void sol()
{
     output_low(led1x1);
     output_low(led1x2); 
     output_low(led1x3); 
     output_high(led2x1); 
     output_low(led2x2); 
     output_low(led2x3); 
     output_low(led3x1); 
     output_low(led3x2); 
     output_low(led3x3); 
}
void sagalt()
{
     output_low(led1x1);
     output_low(led1x2); 
     output_low(led1x3); 
     output_low(led2x1); 
     output_low(led2x2); 
     output_low(led2x3); 
     output_low(led3x1); 
     output_low(led3x2); 
     output_high(led3x3); 
}
void alt()
{
     output_low(led1x1);
     output_low(led1x2); 
     output_low(led1x3); 
     output_low(led2x1); 
     output_low(led2x2); 
     output_low(led2x3); 
     output_low(led3x1); 
     output_high(led3x2); 
     output_low(led3x3); 
}
void solalt()

{
     output_low(led1x1);
     output_low(led1x2); 
     output_low(led1x3); 
     output_low(led2x1); 
     output_low(led2x2); 
     output_low(led2x3); 
     output_high(led3x1); 
     output_low(led3x2); 
     output_low(led3x3); 
}



#int_rda
void serihaberlesme_kesmesi()
{
   disable_interrupts(int_rda);
   gelenVeri = getchar();
   sayma=1;
  
   //////////////////////////////////////////////////////
   //////////////////////////////////////////////////////
   //////////////////////////////////////////////////////
   //////////////////////////////////////////////////////
   // -------------------- MANUEL KONTROL ---------------
   
   // ---- YUKARI ---
   if(gelenVeri=='U')
   {
      output_high(stepZ_dir);
      for(a=0; a<adim1; a++)
      {  output_high(stepZ_step); delay_us(pulseSure1); output_low(stepZ_step); delay_us(pulseSure1);  }
   }
   // ---- A�A�I ---
      if(gelenVeri=='O')
   {
      output_low(stepZ_dir);
      for(c=0; c<adim1; c++)
      {  output_high(stepZ_step); delay_us(pulseSure1); output_low(stepZ_step); delay_us(pulseSure1);  }
   }
   // ----- SOLA ----
      if(gelenVeri=='J')
   {
      output_low(stepX_dir);
      for(d=0; d<adim1; d++)
      {  output_high(stepX_step); delay_us(pulseSure1); output_low(stepX_step); delay_us(pulseSure1);  }
   }
   // ----- SA�A ------
      if(gelenVeri=='W')
   {
      output_high(stepX_dir);
      for(e=0; e<adim1; e++)
      {  output_high(stepX_step); delay_us(pulseSure1); output_low(stepX_step); delay_us(pulseSure1);  }
   }
   //////////////////////////////////////////////////////
   //////////////////////////////////////////////////////
   //////////////////////////////////////////////////////
   //////////////////////////////////////////////////////
   
    // --------------------- SAG USTE G�T --------------------
   if(gelenVeri=='C') 
   {  
      xSayac=xSayac+adim;
      zSayac=zSayac+adim;
      output_high(stepZ_dir);
      for(k=0; k<adim; k++)
      {  output_high(stepZ_step); delay_us(pulseSure); output_low(stepZ_step); delay_us(pulseSure);  }
      output_high(stepX_dir);
      for(i=0; i<adim; i++)
      {  output_high(stepX_step); delay_us(pulseSure); output_low(stepX_step); delay_us(pulseSure);  }
   }   
        
   // --------------------- YUKARI  G�T --------------------
   if(gelenVeri=='B') 
   { 
      zSayac=zSayac+adim;
      output_high(stepZ_dir);
      for(k=0; k<adim; k++)
      { output_high(stepZ_step); delay_us(pulseSure); output_low(stepZ_step); delay_us(pulseSure);   }
    }
      
   // --------------------- SOL USTE G�T --------------------
   if(gelenVeri=='A') 
   {  
      xSayac=xSayac-adim;
      zSayac=zSayac+adim;
      output_high(stepZ_dir);
      for(k=0; k<adim; k++)
      { output_high(stepZ_step); delay_us(pulseSure); output_low(stepZ_step); delay_us(pulseSure);  }
       output_low(stepX_dir);
      for(j=0; j<adim; j++)
      { output_high(stepX_step); delay_us(pulseSure); output_low(stepX_step); delay_us(pulseSure);  }   
   }
   // --------------------- SAGA G�T --------------------
   if(gelenVeri=='F') 
   { 
      xSayac=xSayac+adim;
      output_high(stepX_dir);
      for(i=0; i<adim; i++)
      { output_high(stepX_step); delay_us(pulseSure); output_low(stepX_step); delay_us(pulseSure); }
   }
      
   // --------------------- BEKLE --------------------
   if(gelenVeri=='E') 
   {  
     // BEKLE................

   }
   // --------------------- SOLA G�T --------------------
   if(gelenVeri=='D') 
   { 
      xSayac=xSayac-adim;
      output_low(stepX_dir);
      for(j=0; j<adim; j++)
      {  output_high(stepX_step); delay_us(pulseSure); output_low(stepX_step); delay_us(pulseSure); } 
   }
   // --------------------- SAG ALTA G�T --------------------
   if(gelenVeri=='K') 
   {  
      xSayac=xSayac+adim;
      zSayac=zSayac-adim;
      output_low(stepZ_dir);
      for(b=0; b<adim; b++)
      { output_high(stepZ_step); delay_us(pulseSure); output_low(stepZ_step); delay_us(pulseSure); }   
       output_high(stepX_dir);
      for(i=0; i<adim; i++)
      {   output_high(stepX_step); delay_us(pulseSure); output_low(stepX_step); delay_us(pulseSure); }    
   }
   // --------------------- ALTA G�T --------------------
   if(gelenVeri=='H') 
   {  
      zSayac=zSayac-adim;
      output_low(stepZ_dir);
      for(k=0; k<adim; k++)
      { output_high(stepZ_step); delay_us(pulseSure); output_low(stepZ_step); delay_us(pulseSure);  }

   }
   // --------------------- SOL ALTA G�T --------------------
   if(gelenVeri=='G') 
   {  
      zSayac=zSayac-adim;
      xSayac=xSayac-adim;
      output_low(stepZ_dir);
      for(b=0; b<adim; b++)
      {  output_high(stepZ_step); delay_us(pulseSure); output_low(stepZ_step); delay_us(pulseSure); }
      output_low(stepX_dir);
      for(j=0; j<adim; j++)
      { output_high(stepX_step); delay_us(pulseSure); output_low(stepX_step); delay_us(pulseSure);  }
   }
           
    // ------------------------ EVE G�T ---------------------------------
    if(gelenVeri=='I'&& sayma==1)
    {
    // X sayac eksi de�er ise saga dogru gitsin
     if(abs(xSayac)!=xSayac)
     {   
         adimSag=abs(xSayac);
         output_high(stepX_dir);
       for(n=0; n<adimSag; n++)
        {  output_high(stepX_step); delay_us(pulseSure3); output_low(stepX_step); delay_us(pulseSure3);  }
        
    
    }
     // Z sayac eksi de�er ise yukar� c�ks�n
     if(abs(zSayac)!=zSayac)
     { 
     adimUst=abs(zSayac);
        output_high(stepZ_dir);
   for(f=0; f<adimUst; f++)
   {  output_high(stepZ_step); delay_us(pulseSure3); output_low(stepZ_step); delay_us(pulseSure3);  }
   
  
   }
     // X sayac art� de�er ise sola dogru gitsin
     if(abs(xSayac)==xSayac)
     { adimSol=xSayac; 
         output_low(stepX_dir);
    for(h=0; h<adimSol; h++)
    {  output_high(stepX_step); delay_us(pulseSure3); output_low(stepX_step); delay_us(pulseSure3);  }
    
   
    }
     // Z sayac art� deger ise asag� insin 
     if(abs(zSayac)==zSayac)
     { 
     adimAlt=zSayac;
         output_low(stepZ_dir);
    for(g=0; g<adimAlt; g++)
    {  output_high(stepZ_step); delay_us(pulseSure3); output_low(stepZ_step); delay_us(pulseSure3);  }
    
    }
    
    adimUst=0;  adimAlt=0;  adimSol=0;  adimSag=0; zSayac=0; xSayac=0; sayma=0;
      
    }
   // -----------------------------------------------------------------
   
      // --------------------------- LED YAK -----------------------------
   if(gelenVeri=='M')  { solust(); }
   if(gelenVeri=='N')  { ust();    }
   if(gelenVeri=='S')  { sagust(); }
   if(gelenVeri=='L')  { sol();    }
   if(gelenVeri=='R')  { orta();   }
   if(gelenVeri=='P')  { sag();    }
   if(gelenVeri=='T')  { solalt(); }
   if(gelenVeri=='Y')  { alt();    }
   if(gelenVeri=='Z')  { sagalt(); }  
   
   // -----------------------------------------------------------------
}


void main()
{ 
     enable_interrupts(GLOBAL);
     output_high(stepX_E);
     output_high(stepZ_E);
     output_high(led1x1);
     output_high(led1x2); 
     output_high(led1x3); 
     output_high(led2x1); 
     output_high(led2x2); 
     output_high(led2x3); 
     output_high(led3x1); 
     output_high(led3x2); 
     output_high(led3x3);
     delay_ms(1000);
     output_low(led1x1);
     output_low(led1x2); 
     output_low(led1x3); 
     output_low(led2x1); 
     output_low(led2x2); 
     output_low(led2x3); 
     output_low(led3x1); 
     output_low(led3x2); 
     output_low(led3x3);
   
   while(TRUE)
   {
      
      enable_interrupts(int_rda);
      output_low(stepX_E);
      output_low(stepZ_E);
      
   }  
}









