�� 
 D E C L A R E   @ s q l   n v a r c h a r ( 2 5 5 )  
 S E L E C T   @ s q l   =   ' A L T E R   T A B L E   [ z o n e s _ f t z 2 1 4 ] . [ d o c u m e n t ]   D R O P   C O N S T R A I N T   '   +   d e f a u l t _ c o n s t r a i n t s . n a m e  
 F R O M    
         s y s . a l l _ c o l u m n s  
  
                 I N N E R   J O I N  
         s y s . t a b l e s  
                 O N   a l l _ c o l u m n s . o b j e c t _ i d   =   t a b l e s . o b j e c t _ i d  
  
                 I N N E R   J O I N    
         s y s . s c h e m a s  
                 O N   t a b l e s . s c h e m a _ i d   =   s c h e m a s . s c h e m a _ i d  
  
                 I N N E R   J O I N  
         s y s . d e f a u l t _ c o n s t r a i n t s  
                 O N   a l l _ c o l u m n s . d e f a u l t _ o b j e c t _ i d   =   d e f a u l t _ c o n s t r a i n t s . o b j e c t _ i d  
  
 W H E R E    
                 s c h e m a s . n a m e   =   ' z o n e s _ f t z 2 1 4 '  
         A N D   t a b l e s . n a m e   =   ' d o c u m e n t '  
         A N D   a l l _ c o l u m n s . n a m e   =   ' S t a t u s '  
  
 	 E X E C U T E   s p _ e x e c u t e s q l   @ s q l  
 	 A L T E R   T A B L E   [ z o n e s _ f t z 2 1 4 ] . [ f i l i n g _ h e a d e r ]  
         D R O P   C O L U M N   I F   E X I S T S     [ r e q u e s t _ x m l ] ,  
                                 [ r e s p o n s e _ x m l ] ,  
                                 [ e r r o r _ d e s c r i p t i o n ]  
         A L T E R   T A B L E   [ z o n e s _ f t z 2 1 4 ] . [ d o c u m e n t ]  
         D R O P   C O L U M N   I F   E X I S T S     [ S t a t u s ] ,  
         	 	 	 [ r e q u e s t _ x m l ] ,  
         	 	 	 [ r e s p o n s e _ x m l ] 