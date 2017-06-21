-- Table: "Web"."ComponentType"

-- DROP TABLE "Web"."ComponentType";

CREATE TABLE "Web"."ComponentType"
(
    id integer NOT NULL,
    "Type" character varying(50)[] COLLATE pg_catalog."default" NOT NULL,
    CONSTRAINT "ComponentType_pkey" PRIMARY KEY (id)
)
WITH (
    OIDS = FALSE
)
TABLESPACE pg_default;