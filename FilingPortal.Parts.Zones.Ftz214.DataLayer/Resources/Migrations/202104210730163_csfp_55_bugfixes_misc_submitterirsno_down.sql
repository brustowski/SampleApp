�� 
  
 A L T E R       P R O C E D U R E   [ z o n e s _ f t z 2 1 4 ] . [ s p _ a d d _ m i s c ]   ( @ f i l i n g H e a d e r I d   I N T ,  
 @ p a r e n t I d   I N T ,  
 @ f i l i n g U s e r   N V A R C H A R ( 2 5 5 )   =   N U L L ,  
 @ o p e r a t i o n I d   U N I Q U E I D E N T I F I E R   =   N U L L   O U T P U T )  
 A S  
 B E G I N  
     S E T   N O C O U N T   O N ;  
  
     D E C L A R E   @ t a b l e N a m e   V A R C H A R ( 1 2 8 )   =   ' m i s c ' ;  
     D E C L A R E   @ a l l o w M u l t i p l e   B I T   =   0 ;  
  
     S E T   @ o p e r a t i o n I d   =   C O A L E S C E ( @ o p e r a t i o n I d ,   N E W I D ( ) ) ;  
  
     - -   g e t   s e c t i o n   p r o p e r t y   i s _ a r r a y  
     S E L E C T  
         @ a l l o w M u l t i p l e   =   p s . i s _ a r r a y  
     F R O M   z o n e s _ f t z 2 1 4 . f o r m _ s e c t i o n _ c o n f i g u r a t i o n   p s  
     W H E R E   p s . t a b l e _ n a m e   =   @ t a b l e N a m e ;  
  
     - -   a d d   m i s c   d a t a   a n d   a p p l y   r u l e s  
     I F   @ a l l o w M u l t i p l e   =   1  
         O R   N O T   E X I S T S   ( S E L E C T  
                 1  
             F R O M   z o n e s _ f t z 2 1 4 . m i s c   m i s c  
             W H E R E   m i s c . f i l i n g _ h e a d e r _ i d   =   @ f i l i n g H e a d e r I d )  
     B E G I N  
         I N S E R T   I N T O   z o n e s _ f t z 2 1 4 . m i s c   ( f i l i n g _ h e a d e r _ i d  
 	 	 , p a r e n t _ r e c o r d _ i d  
 	 	 , o p e r a t i o n _ i d  
 	 	 , b r a n c h  
 	 	 , [ b r o k e r ]  
 	 	 , c r e a t e d _ d a t e  
 	 	 , c r e a t e d _ u s e r  
 	 	 , [ s e r v i c e ]  
 	 	 , s u b m i t t e r )  
             S E L E C T   D I S T I N C T  
                 @ f i l i n g H e a d e r I d  
               , @ p a r e n t I d  
               , @ o p e r a t i o n I d  
               , u s e r _ d a t a . B r a n c h  
               , u s e r _ d a t a . [ B r o k e r ]  
               , G E T D A T E ( )  
               , @ f i l i n g U s e r  
 	       , t r a n s p o r t M o d e . s e r v i c e _ c o d e  
 	       , C l i e n t s . C l i e n t C o d e  
             F R O M   z o n e s _ f t z 2 1 4 . f i l i n g _ d e t a i l   d e t a i l  
             J O I N   z o n e s _ f t z 2 1 4 . i n b o u n d   i n b n d  
                 O N   i n b n d . i d   =   d e t a i l . i n b o u n d _ i d  
             L E F T   J O I N   z o n e s _ f t z 2 1 4 . i n b o u n d _ p a r s e d _ d a t a   p a r s e d _ d a t a  
 	 	 O N   i n b n d . i d   =   p a r s e d _ d a t a . i d  
             L E F T   J O I N   d b o . a p p _ u s e r s _ d a t a   u s e r _ d a t a  
                 O N   u s e r _ d a t a . U s e r A c c o u n t   =   @ f i l i n g U s e r  
 	     J O I N   h a n d b o o k _ t r a n s p o r t _ m o d e   A S   t r a n s p o r t M o d e  
 	 	 O N   t r a n s p o r t M o d e . c o d e _ n u m b e r   =   p a r s e d _ d a t a . m o t  
 	 	 	 	 L E F T   J O I N   d b o . C l i e n t s   A S   c l i e n t s  
 	 	 O N   i n b n d . a p p l i c a n t _ i d   =   c l i e n t s . i d  
             W H E R E   d e t a i l . f i l i n g _ h e a d e r _ i d   =   @ f i l i n g H e a d e r I d  
     E N D ;  
 E N D ;  
 