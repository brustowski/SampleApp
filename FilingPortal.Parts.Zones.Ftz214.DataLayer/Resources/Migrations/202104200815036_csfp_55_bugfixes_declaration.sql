�� 
  
  
 - -   a d d   d e c l a r a t i o n   r e c o r d   - -  
 A L T E R       P R O C E D U R E   [ z o n e s _ f t z 2 1 4 ] . [ s p _ a d d _ d e c l a r a t i o n ]   (  
 	 @ f i l i n g H e a d e r I d   I N T ,  
 	 @ p a r e n t I d   I N T ,  
 	 @ f i l i n g U s e r   N V A R C H A R ( 2 5 5 )   =   N U L L ,  
 	 @ o p e r a t i o n I d   U N I Q U E I D E N T I F I E R   =   N U L L   O U T P U T )  
 A S  
 B E G I N  
 	 S E T   N O C O U N T   O N ;  
  
 	 D E C L A R E   @ t a b l e N a m e   V A R C H A R ( 1 2 8 )   =   ' d e c l a r a t i o n '  
 	 D E C L A R E   @ a l l o w M u l t i p l e   B I T   =   0 ;  
  
 	 S E T   @ o p e r a t i o n I d   =   C O A L E S C E ( @ o p e r a t i o n I d ,   N E W I D ( ) ) ;  
  
 	 - -   g e t   s e c t i o n   p r o p e r t y   i s _ a r r a y  
 	 S E L E C T   @ a l l o w M u l t i p l e   =   s e c t i o n . i s _ a r r a y  
 	 F R O M   z o n e s _ f t z 2 1 4 . f o r m _ s e c t i o n _ c o n f i g u r a t i o n   s e c t i o n  
 	 W H E R E   s e c t i o n . t a b l e _ n a m e   =   @ t a b l e N a m e  
  
 	 - -   a d d   d e c l a r a t i o n   d a t a  
 	 I F   @ a l l o w M u l t i p l e   =   1   O R   N O T   E X I S T S   (  
 	 	 S E L E C T   1  
 	 	 F R O M   z o n e s _ f t z 2 1 4 . d e c l a r a t i o n   d e c l a r a t i o n  
 	 	 W H E R E   d e c l a r a t i o n . f i l i n g _ h e a d e r _ i d   =   @ f i l i n g H e a d e r I d  
 	 )  
 	 B E G I N  
 	 	 I N S E R T   I N T O   z o n e s _ f t z 2 1 4 . d e c l a r a t i o n   (  
 	 	 	   f i l i n g _ h e a d e r _ i d  
 	 	 	 , p a r e n t _ r e c o r d _ i d  
 	 	 	 , o p e r a t i o n _ i d  
 	 	 	 , i m p o r t e r  
 	 	 	 , t r a n s p o r t  
 	 	 	 , c o n t a i n e r  
 	 	 	 , a d m i s s i o n _ t y p e  
 	 	 	 , d i r e c t _ d e l i v e r y  
 	 	 	 , i s s u e r  
 	 	 	 , o c e a n _ b i l l  
 	 	 	 , v e s s e l  
 	 	 	 , v o y a g e  
 	 	 	 , l o a d i n g _ p o r t  
 	 	 	 , d i s c h a r g e _ p o r t  
 	 	 	 , f t z _ p o r t  
 	 	 	 , c a r r i e r _ s c a c  
 	 	 	 , d e p  
 	 	 	 , a r r  
 	 	 	 , a r r 2  
 	 	 	 , h m f  
 	 	 	 , f i r s t _ a r r _ d a t e  
 	 	 	 , d e s c r i p t i o n  
 	 	 	 , f i r m s _ c o d e  
 	 	 	 , z o n e _ i d  
 	 	 	 , y e a r  
 	 	 	 , a p p l i c a n t  
 	 	 	 , f t z _ o p e r a t o r  
 	 	 	 , c r e a t e d _ d a t e  
 	 	 	 , c r e a t e d _ u s e r  
 	 	 	 , i t _ n o  
 	 	 	 , i t _ d a t e  
 	 	 	 , a d m i s s i o n _ n o  
 	 	 	 , u l t i m a t e _ c o n s i g n e e  
 	 	 )  
 	 	 S E L E C T  
 	 	 	   @ f i l i n g H e a d e r I d  
 	 	 	 , @ p a r e n t I d  
 	 	 	 , @ o p e r a t i o n I d  
 	 	 	 , c l i e n t s . C l i e n t C o d e  
 	 	 	 , t r a n s p o r t M o d e . c o d e  
 	 	 	 , t r a n s p o r t M o d e . c o n t a i n e r _ c o d e  
 	 	 	 , i n b n d . a d m i s s i o n _ t y p e  
 	 	 	 , d i r e c t _ d e l i v e r y  
 	 	 	 , p a r s e d _ d a t a . i m p _ c a r r i e r _ c o d e  
 	 	 	 , p a r s e d _ d a t a . m a s t e r  
 	 	 	 , p a r s e d _ d a t a . i m p _ v e s s e l  
 	 	 	 , p a r s e d _ d a t a . f l t _ v o y _ t r i p   - - v o y a g e  
 	 	 	 , p a r s e d _ d a t a . f o r e i g n _ p o r t  
 	 	 	 , p a r s e d _ d a t a . u n l a d i n g _ p o r t  
 	 	 	 , p a r s e d _ d a t a . z o n e _ p o r t  
 	 	 	 , p a r s e d _ d a t a . i m p _ c a r r i e r _ c o d e   - -   c a r r i e r _ s c a c  
 	 	 	 , p a r s e d _ d a t a . e x p o r t _ d a t e  
 	 	 	 , p a r s e d _ d a t a . i m p o r t _ d a t e  
 	 	 	 , p a r s e d _ d a t a . i m p o r t _ d a t e    
 	 	 	 , p a r s e d _ l i n e . h m f   - -   h m f  
 	 	 	 , p a r s e d _ d a t a . e s t _ a r r _ d a t e  
 	 	 	 , p a r s e d _ l i n e . d e s c r i p t i o n  
 	 	 	 , p a r s e d _ d a t a . p t t _ f i r m s  
 	 	 	 , i n b n d . z o n e _ i d  
 	 	 	 , p a r s e d _ d a t a . a d m i s s i o n _ y e a r  
 	 	 	 , c l i e n t s . C l i e n t C o d e  
 	 	 	 , c l i e n t s . C l i e n t C o d e  
 	 	 	 , G E T D A T E ( )  
 	 	 	 , @ f i l i n g U s e r  
 	 	 	 , p a r s e d _ d a t a . i t _ n o  
 	 	 	 , p a r s e d _ d a t a . i t _ d a t e  
 	 	 	 , p a r s e d _ d a t a . a d m i s s i o n _ n o  
 	 	 	 , c l i e n t s . C l i e n t C o d e  
  
 	 	 F R O M   z o n e s _ f t z 2 1 4 . f i l i n g _ d e t a i l   A S   d e t a i l  
  
 	 	 J O I N   z o n e s _ f t z 2 1 4 . i n b o u n d   A S   i n b n d  
 	 	 O N   i n b n d . i d   =   d e t a i l . i n b o u n d _ i d  
 	 	  
 	 	 L E F T   J O I N   z o n e s _ f t z 2 1 4 . i n b o u n d _ p a r s e d _ d a t a   p a r s e d _ d a t a  
 	 	 O N   i n b n d . i d   =   p a r s e d _ d a t a . i d  
  
 	 	 L E F T   J O I N   z o n e s _ f t z 2 1 4 . i n b o u n d _ p a r s e d _ l i n e   p a r s e d _ l i n e  
 	 	 O N   i n b n d . i d   =   p a r s e d _ l i n e . i d  
  
 	 	 L E F T   J O I N   d b o . C l i e n t s   A S   c l i e n t s  
 	 	 O N   i n b n d . a p p l i c a n t _ i d   =   c l i e n t s . i d  
  
 	 	 J O I N   h a n d b o o k _ t r a n s p o r t _ m o d e   A S   t r a n s p o r t M o d e  
 	 	 O N   t r a n s p o r t M o d e . c o d e _ n u m b e r   =   p a r s e d _ d a t a . m o t  
  
  
 	 	 W H E R E   d e t a i l . f i l i n g _ h e a d e r _ i d   =   @ f i l i n g H e a d e r I d  
 	 E N D  
 E N D ;  
 