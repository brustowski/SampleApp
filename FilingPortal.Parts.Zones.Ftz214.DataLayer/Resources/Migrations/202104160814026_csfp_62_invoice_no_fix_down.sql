�� 
 - -   a d d   t r u c k   i m p o r t   i n v o i c e   h e a d e r   r e c o r d   - -  
 A L T E R       P R O C E D U R E   [ z o n e s _ f t z 2 1 4 ] . [ s p _ a d d _ i n v o i c e _ h e a d e r ]   ( @ f i l i n g H e a d e r I d   I N T ,  
 @ p a r e n t I d   I N T ,  
 @ f i l i n g U s e r   N V A R C H A R ( 2 5 5 )   =   N U L L ,  
 @ o p e r a t i o n I d   U N I Q U E I D E N T I F I E R   =   N U L L   O U T P U T )  
 A S  
 B E G I N  
     S E T   N O C O U N T   O N ;  
  
     D E C L A R E   @ t a b l e N a m e   V A R C H A R ( 1 2 8 )   =   ' i n v o i c e _ h e a d e r ' ;  
     D E C L A R E   @ a l l o w M u l t i p l e   B I T   =   0 ;  
     D E C L A R E   @ I D s   T A B L E   (  
         I D   I N T  
     ) ;  
  
     S E T   @ o p e r a t i o n I d   =   C O A L E S C E ( @ o p e r a t i o n I d ,   N E W I D ( ) ) ;  
  
     - -   g e t   s e c t i o n   p r o p e r t y   i s _ a r r a y  
     S E L E C T  
         @ a l l o w M u l t i p l e   =   s e c t i o n . i s _ a r r a y  
     F R O M   z o n e s _ f t z 2 1 4 . f o r m _ s e c t i o n _ c o n f i g u r a t i o n   s e c t i o n  
     W H E R E   s e c t i o n . t a b l e _ n a m e   =   @ t a b l e N a m e  
  
     - -   a d d   i n v o i c e   h e a d e r   d a t a   a n d   a p p l y   r u l e s  
     I F   @ a l l o w M u l t i p l e   =   1  
         O R   N O T   E X I S T S   ( S E L E C T  
                 1  
             F R O M   z o n e s _ f t z 2 1 4 . i n v o i c e _ h e a d e r   i n v o i c e _ h e a d e r  
             W H E R E   i n v o i c e _ h e a d e r . f i l i n g _ h e a d e r _ i d   =   @ f i l i n g H e a d e r I d )  
     B E G I N  
         I N S E R T   I N T O   z o n e s _ f t z 2 1 4 . i n v o i c e _ h e a d e r   (  
 	   f i l i n g _ h e a d e r _ i d  
         , p a r e n t _ r e c o r d _ i d  
         , o p e r a t i o n _ i d  
         , c r e a t e d _ d a t e  
         , c r e a t e d _ u s e r  
 	 , i m p o r t e r  
 	 , c o n s i g n e e  
 	 , s h i p _ t o _ p a r t y )  
         O U T P U T   I N S E R T E D . I D   I N T O   @ I D s   ( I D )  
             S E L E C T   D I S T I N C T  
                 @ f i l i n g H e a d e r I d  
               , @ p a r e n t I d  
               , @ o p e r a t i o n I d  
               , G E T D A T E ( )  
               , @ f i l i n g U s e r  
 	       , C l i e n t s . C l i e n t C o d e  
 	       , C l i e n t s . C l i e n t C o d e  
 	       , C l i e n t s . C l i e n t C o d e  
             F R O M   z o n e s _ f t z 2 1 4 . f i l i n g _ d e t a i l   d e t a i l  
             J O I N   z o n e s _ f t z 2 1 4 . i n b o u n d   i n b n d  
                 O N   i n b n d . i d   =   d e t a i l . i n b o u n d _ i d  
 	     L E F T   J O I N   d b o . C l i e n t s   A S   c l i e n t s  
 	 	 O N   i n b n d . a p p l i c a n t _ i d   =   c l i e n t s . i d  
             W H E R E   d e t a i l . f i l i n g _ h e a d e r _ i d   =   @ f i l i n g H e a d e r I d  
  
         D E C L A R E   @ r e c o r d I d   I N T  
         D E C L A R E   c u r   C U R S O R   F A S T _ F O R W A R D   R E A D _ O N L Y   L O C A L   F O R   S E L E C T  
             I D  
         F R O M   @ I D s  
  
         O P E N   c u r  
  
         F E T C H   N E X T   F R O M   c u r   I N T O   @ r e c o r d I d  
         W H I L E   @ @ F E T C H _ S T A T U S   =   0  
         B E G I N  
  
         - -   a d d   i n v o i c e   l i n e  
         E X E C   z o n e s _ f t z 2 1 4 . s p _ a d d _ i n v o i c e _ l i n e   @ f i l i n g H e a d e r I d  
                                                                                 , @ r e c o r d I d  
                                                                                 , @ f i l i n g U s e r  
                                                                                 , @ o p e r a t i o n I d  
  
         F E T C H   N E X T   F R O M   c u r   I N T O   @ r e c o r d I d  
  
         E N D  
  
         C L O S E   c u r  
         D E A L L O C A T E   c u r  
  
     E N D ;  
 E N D ;  
 